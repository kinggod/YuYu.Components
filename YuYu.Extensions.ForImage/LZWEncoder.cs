using System;
using System.IO;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    internal class LZWEncoder
    {
        private const int _EOF = -1;
        private int _ImgW, _ImgH;
        private byte[] _PixAry;
        private int _InitCodeSize;
        private int _Remaining;
        private int _CurPixel;

        // GIFCOMPR.C       - GIF Image compression routines
        //
        // Lempel-Ziv compression based on 'compress'.  GIF modifications by
        // David Rowley (mgardi@watdcsu.waterloo.edu)

        // General DEFINEs

        private const int _BITS = 12;
        private const int _HSIZE = 5003; // 80% occupancy
        // GIF Image compression - modified 'compress'
        //
        // Based on: compress.c - File compression ala IEEE Computer, June 1984.
        //
        // By Authors:  Spencer W. Thomas      (decvax!harpo!utah-cs!utah-gr!thomas)
        //              Jim McKie              (decvax!mcvax!jim)
        //              Steve Davies           (decvax!vax135!petsd!peora!srd)
        //              Ken Turkowski          (decvax!decwrl!turtlevax!ken)
        //              James A. Woods         (decvax!ihnp4!ames!jaw)
        //              Joe Orost              (decvax!vax135!petsd!joe)

        private int _N_Bits; // number of bits/code
        private int _MaxBits = _BITS; // user settable max # bits/code
        private int _MaxCode; // maximum code, given n_bits
        private int _MaxMaxCode = 1 << _BITS; // should NEVER generate this code
        private int[] _Htab = new int[_HSIZE];
        private int[] _CodeTab = new int[_HSIZE];
        private int _HSize = _HSIZE; // for dynamic table sizing
        private int _Free_Ent = 0; // first unused entry
        // block compression parameters -- after all codes are used up,
        // and compression rate changes, start over.
        private bool _Clear_Flg = false;

        // Algorithm:  use open addressing double hashing (no chaining) on the
        // prefix code / next character combination.  We do a variant of Knuth's
        // algorithm D (vol. 3, sec. 6.4) along with G. Knott's relatively-prime
        // secondary probe.  Here, the modular division first probe is gives way
        // to a faster exclusive-or manipulation.  Also do block compression with
        // an adaptive reset, whereby the code table is cleared when the compression
        // ratio decreases, but after the table fills.  The variable-length output
        // codes are re-sized at this point, and a special CLEAR code is generated
        // for the decompressor.  Late addition:  construct the table according to
        // file size for noticeable speed improvement on small files.  Please direct
        // questions about this implementation to ames!jaw.

        private int _G_Init_Bits;

        private int _ClearCode;
        private int _EOFCode;

        // output
        //
        // Output the given code.
        // Inputs:
        //      code:   A n_bits-bit integer.  If == -1, then EOF.  This assumes
        //              that n_bits =< wordsize - 1.
        // Outputs:
        //      Outputs code to the file.
        // Assumptions:
        //      Chars are 8 bits long.
        // Algorithm:
        //      Maintain a BITS character long buffer (so that 8 codes will
        // fit in it exactly).  Use the VAX insv instruction to insert each
        // code in turn.  When the buffer fills up empty it and start over.

        private int _Cur_Accum = 0;
        private int _Cur_Bits = 0;

        private int[] _Masks = { 0x0000, 0x0001, 0x0003, 0x0007, 0x000F, 0x001F, 0x003F, 0x007F, 0x00FF, 0x01FF, 0x03FF, 0x07FF, 0x0FFF, 0x1FFF, 0x3FFF, 0x7FFF, 0xFFFF };

        // Number of characters so far in this 'packet'
        private int _A_Count;

        // Define the storage for the packet accumulator
        private byte[] _Accum = new byte[256];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixels"></param>
        /// <param name="color_depth"></param>
        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            _ImgW = width;
            _ImgH = height;
            _PixAry = pixels;
            _InitCodeSize = Math.Max(2, color_depth);
        }

        /// <summary>
        /// Add a character to the end of the current packet, and if it is 254 characters, flush the packet to disk.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="outs"></param>
        private void Add(byte c, Stream outs)
        {
            _Accum[_A_Count++] = c;
            if (_A_Count >= 254)
                _Flush(outs);
        }

        /// <summary>
        /// Clear out the hash table.table clear for block compress.
        /// </summary>
        /// <param name="outs"></param>
        private void _ClearTable(Stream outs)
        {
            _ResetCodeTable(_HSize);
            _Free_Ent = _ClearCode + 2;
            _Clear_Flg = true;
            _Output(_ClearCode, outs);
        }

        /// <summary>
        ///  reset code table
        /// </summary>
        /// <param name="hsize"></param>
        private void _ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; ++i)
                _Htab[i] = -1;
        }

        private void _Compress(int init_bits, Stream outs)
        {
            int fcode, i, c, ent, disp, hsize_reg, hshift;
            // Set up the globals:  g_init_bits - initial number of bits
            _G_Init_Bits = init_bits;
            // Set up the necessary values
            _Clear_Flg = false;
            _N_Bits = _G_Init_Bits;
            _MaxCode = _GetMaxCode(_N_Bits);
            _ClearCode = 1 << (init_bits - 1);
            _EOFCode = _ClearCode + 1;
            _Free_Ent = _ClearCode + 2;
            _A_Count = 0; // clear packet
            ent = _NextPixel();
            hshift = 0;
            for (fcode = _HSize; fcode < 65536; fcode *= 2)
                ++hshift;
            hshift = 8 - hshift; // set hash code range bound
            hsize_reg = _HSize;
            _ResetCodeTable(hsize_reg); // clear hash table
            _Output(_ClearCode, outs);
        outer_loop: while ((c = _NextPixel()) != _EOF)
            {
                fcode = (c << _MaxBits) + ent;
                i = (c << hshift) ^ ent; // xor hashing
                if (_Htab[i] == fcode)
                {
                    ent = _CodeTab[i];
                    continue;
                }
                else if (_Htab[i] >= 0) // non-empty slot
                {
                    disp = hsize_reg - i; // secondary hash (after G. Knott)
                    if (i == 0)
                        disp = 1;
                    do
                    {
                        if ((i -= disp) < 0)
                            i += hsize_reg;
                        if (_Htab[i] == fcode)
                        {
                            ent = _CodeTab[i];
                            goto outer_loop;
                        }
                    } while (_Htab[i] >= 0);
                }
                _Output(ent, outs);
                ent = c;
                if (_Free_Ent < _MaxMaxCode)
                {
                    _CodeTab[i] = _Free_Ent++; // code -> hashtable
                    _Htab[i] = fcode;
                }
                else
                    _ClearTable(outs);
            }
            // Put out the final code.
            _Output(ent, outs);
            _Output(_EOFCode, outs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"></param>
        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(_InitCodeSize)); // write "initial code size" byte
            _Remaining = _ImgW * _ImgH; // reset navigation variables
            _CurPixel = 0;
            _Compress(_InitCodeSize + 1, os); // compress and write the pixel data
            os.WriteByte(0); // write block terminator
        }

        /// <summary>
        /// Flush the packet to disk, and reset the accumulator
        /// </summary>
        /// <param name="outs"></param>
        private void _Flush(Stream outs)
        {
            if (_A_Count > 0)
            {
                outs.WriteByte(Convert.ToByte(_A_Count));
                outs.Write(_Accum, 0, _A_Count);
                _A_Count = 0;
            }
        }

        private int _GetMaxCode(int n_bits)
        {
            return (1 << n_bits) - 1;
        }

        /// <summary>
        /// Return the next pixel from the image
        /// </summary>
        /// <returns></returns>
        private int _NextPixel()
        {
            if (_Remaining == 0)
                return _EOF;
            --_Remaining;
            int temp = _CurPixel + 1;
            if (temp < _PixAry.GetUpperBound(0))
            {
                byte pix = _PixAry[_CurPixel++];
                return pix & 0xff;
            }
            return 0xff;
        }

        private void _Output(int code, Stream outs)
        {
            _Cur_Accum &= _Masks[_Cur_Bits];
            if (_Cur_Bits > 0)
                _Cur_Accum |= (code << _Cur_Bits);
            else
                _Cur_Accum = code;
            _Cur_Bits += _N_Bits;
            while (_Cur_Bits >= 8)
            {
                Add((byte)(_Cur_Accum & 0xff), outs);
                _Cur_Accum >>= 8;
                _Cur_Bits -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if (_Free_Ent > _MaxCode || _Clear_Flg)
            {
                if (_Clear_Flg)
                {
                    _MaxCode = _GetMaxCode(_N_Bits = _G_Init_Bits);
                    _Clear_Flg = false;
                }
                else
                {
                    ++_N_Bits;
                    if (_N_Bits == _MaxBits)
                        _MaxCode = _MaxMaxCode;
                    else
                        _MaxCode = _GetMaxCode(_N_Bits);
                }
            }
            if (code == _EOFCode)
            {
                // At EOF, write the rest of the buffer.
                while (_Cur_Bits > 0)
                {
                    Add((byte)(_Cur_Accum & 0xff), outs);
                    _Cur_Accum >>= 8;
                    _Cur_Bits -= 8;
                }
                _Flush(outs);
            }
        }
    }
}