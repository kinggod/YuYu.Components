using System;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    internal class NeuQuant
    {
        /// <summary>
        /// number of colours used
        /// </summary>
        protected static readonly int _Netsize = 256;
        /* four primes near 500 - assume no image has a length so large */
        /* that it is divisible by all four primes */
        protected static readonly int _Prime1 = 499;
        protected static readonly int _Prime2 = 491;
        protected static readonly int _Prime3 = 487;
        protected static readonly int _Prime4 = 503;
        protected static readonly int _MinPictureBytes = (3 * _Prime4);
        /* minimum size for input image */
        /* Program Skeleton
           ----------------
           [select samplefac in range 1..30]
           [read image from input file]
           pic = (unsigned char*) malloc(3*width*height);
           initnet(pic,3*width*height,samplefac);
           learn();
           unbiasnet();
           [write output image header, using writecolourmap(f)]
           inxbuild();
           write output image using inxsearch(b,g,r)      */

        /* Network Definitions
           ------------------- */
        protected static readonly int _Maxnetpos = (_Netsize - 1);
        protected static readonly int _NetBiasShift = 4; /* bias for colour values */
        protected static readonly int _Ncycles = 100; /* no. of learning cycles */

        /* defs for freq and bias */
        protected static readonly int _IntBiasShift = 16; /* bias for fractions */
        protected static readonly int _IntBias = (((int)1) << _IntBiasShift);
        protected static readonly int _GammaShift = 10; /* gamma = 1024 */
        protected static readonly int _Gamma = (((int)1) << _GammaShift);
        protected static readonly int _BetaShift = 10;
        protected static readonly int _Beta = (_IntBias >> _BetaShift); /* beta = 1/1024 */
        protected static readonly int _BetaGamma = (_IntBias << (_GammaShift - _BetaShift));

        /* defs for decreasing radius factor */
        protected static readonly int _InitRadius = (_Netsize >> 3); /* for 256 cols, radius starts */
        protected static readonly int _RadiusBiasShift = 6; /* at 32.0 biased by 6 bits */
        protected static readonly int _RadiusBias = (((int)1) << _RadiusBiasShift);
        protected static readonly int _InitRadius2 = (_InitRadius * _RadiusBias); /* and decreases by a */
        protected static readonly int _RadiusDec = 30; /* factor of 1/30 each cycle */

        /* defs for decreasing alpha factor */
        protected static readonly int _AlphaBiasShift = 10; /* alpha starts at 1.0 */
        protected static readonly int _InitAlpha = (((int)1) << _AlphaBiasShift);

        protected int _Alphadec; /* biased by 10 bits */

        /* radbias and alpharadbias used for radpower calculation */
        protected static readonly int _RadiusBiasShift2 = 8;
        protected static readonly int _RadiusBias2 = (((int)1) << _RadiusBiasShift2);
        protected static readonly int _AlphaRadiusShift2 = (_AlphaBiasShift + _RadiusBiasShift2);
        protected static readonly int _AlphaRadiusBias2 = (((int)1) << _AlphaRadiusShift2);

        /* Types and Global Variables
        -------------------------- */

        protected byte[] _ThePicture; /* the input image itself */
        protected int _LengthCount; /* lengthcount = H*W*3 */

        protected int _SampleFac; /* sampling factor 1..30 */

        //   typedef int pixel[4];                /* BGRc */
        protected int[][] _Network; /* the network itself - [netsize][4] */

        protected int[] _NetIndex = new int[256];
        /* for network lookup - really 256 */

        protected int[] _Bias = new int[_Netsize];
        /* bias and freq arrays for learning */
        protected int[] _Freq = new int[_Netsize];
        protected int[] _RadiusPower = new int[_InitRadius];
        /* radpower for precomputation */

        /// <summary>
        /// Initialise network in range (0,0,0) to (255,255,255) and set parameters
        /// </summary>
        /// <param name="thepic"></param>
        /// <param name="len"></param>
        /// <param name="sample"></param>
        public NeuQuant(byte[] thepic, int len, int sample)
        {
            int i;
            int[] p;
            _ThePicture = thepic;
            _LengthCount = len;
            _SampleFac = sample;
            _Network = new int[_Netsize][];
            for (i = 0; i < _Netsize; i++)
            {
                _Network[i] = new int[4];
                p = _Network[i];
                p[0] = p[1] = p[2] = (i << (_NetBiasShift + 8)) / _Netsize;
                _Freq[i] = _IntBias / _Netsize; /* 1/netsize */
                _Bias[i] = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ColorMap()
        {
            byte[] map = new byte[3 * _Netsize];
            int[] index = new int[_Netsize];
            for (int i = 0; i < _Netsize; i++)
                index[_Network[i][3]] = i;
            int k = 0;
            for (int i = 0; i < _Netsize; i++)
            {
                int j = index[i];
                map[k++] = (byte)(_Network[j][0]);
                map[k++] = (byte)(_Network[j][1]);
                map[k++] = (byte)(_Network[j][2]);
            }
            return map;
        }

        /// <summary>
        /// Insertion sort of network and building of netindex[0..255] (to do after unbias)
        /// </summary>
        public void Inxbuild()
        {
            int i, j, smallpos, smallval;
            int[] p;
            int[] q;
            int previouscol, startpos;
            previouscol = 0;
            startpos = 0;
            for (i = 0; i < _Netsize; i++)
            {
                p = _Network[i];
                smallpos = i;
                smallval = p[1]; /* index on g */
                /* find smallest in i..netsize-1 */
                for (j = i + 1; j < _Netsize; j++)
                {
                    q = _Network[j];
                    if (q[1] < smallval)
                    { /* index on g */
                        smallpos = j;
                        smallval = q[1]; /* index on g */
                    }
                }
                q = _Network[smallpos];
                /* swap p (i) and q (smallpos) entries */
                if (i != smallpos)
                {
                    j = q[0];
                    q[0] = p[0];
                    p[0] = j;
                    j = q[1];
                    q[1] = p[1];
                    p[1] = j;
                    j = q[2];
                    q[2] = p[2];
                    p[2] = j;
                    j = q[3];
                    q[3] = p[3];
                    p[3] = j;
                }
                /* smallval entry is now in position i */
                if (smallval != previouscol)
                {
                    _NetIndex[previouscol] = (startpos + i) >> 1;
                    for (j = previouscol + 1; j < smallval; j++)
                        _NetIndex[j] = i;
                    previouscol = smallval;
                    startpos = i;
                }
            }
            _NetIndex[previouscol] = (startpos + _Maxnetpos) >> 1;
            for (j = previouscol + 1; j < 256; j++)
                _NetIndex[j] = _Maxnetpos; /* really 256 */
        }

        /// <summary>
        /// Main Learning Loop
        /// </summary>
        public void Learn()
        {

            int i, j, b, g, r;
            int radius, rad, alpha, step, delta, samplepixels;
            byte[] p;
            int pix, lim;
            if (_LengthCount < _MinPictureBytes)
                _SampleFac = 1;
            _Alphadec = 30 + ((_SampleFac - 1) / 3);
            p = _ThePicture;
            pix = 0;
            lim = _LengthCount;
            samplepixels = _LengthCount / (3 * _SampleFac);
            delta = samplepixels / _Ncycles;
            alpha = _InitAlpha;
            radius = _InitRadius2;
            rad = radius >> _RadiusBiasShift;
            if (rad <= 1)
                rad = 0;
            for (i = 0; i < rad; i++)
                _RadiusPower[i] = alpha * (((rad * rad - i * i) * _RadiusBias2) / (rad * rad));
            //fprintf(stderr,"beginning 1D learning: initial radius=%d\n", rad);

            if (_LengthCount < _MinPictureBytes)
                step = 3;
            else if (_LengthCount % _Prime1 != 0)
                step = 3 * _Prime1;
            else
            {
                if (_LengthCount % _Prime2 != 0)
                    step = 3 * _Prime2;
                else
                {
                    if (_LengthCount % _Prime3 != 0)
                        step = 3 * _Prime3;
                    else
                        step = 3 * _Prime4;
                }
            }

            i = 0;
            while (i < samplepixels)
            {
                b = (p[pix + 0] & 0xff) << _NetBiasShift;
                g = (p[pix + 1] & 0xff) << _NetBiasShift;
                r = (p[pix + 2] & 0xff) << _NetBiasShift;
                j = Contest(b, g, r);
                Altersingle(alpha, j, b, g, r);
                if (rad != 0)
                    Alterneigh(rad, j, b, g, r); /* alter neighbours */
                pix += step;
                if (pix >= lim)
                    pix -= _LengthCount;
                i++;
                if (delta == 0)
                    delta = 1;
                if (i % delta == 0)
                {
                    alpha -= alpha / _Alphadec;
                    radius -= radius / _RadiusDec;
                    rad = radius >> _RadiusBiasShift;
                    if (rad <= 1)
                        rad = 0;
                    for (j = 0; j < rad; j++)
                        _RadiusPower[j] = alpha * (((rad * rad - j * j) * _RadiusBias2) / (rad * rad));
                }
            }
            //fprintf(stderr,"finished 1D learning: readonly alpha=%f !\n",((float)alpha)/initalpha);
        }

        /// <summary>
        /// Search for BGR values 0..255 (after net is unbiased) and return colour index
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public int Map(int b, int g, int r)
        {
            int i, j, dist, a, bestd;
            int[] p;
            int best;
            bestd = 1000; /* biggest possible dist is 256*3 */
            best = -1;
            i = _NetIndex[g]; /* index on g */
            j = i - 1; /* start at netindex[g] and work outwards */
            while (i < _Netsize || j >= 0)
            {
                if (i < _Netsize)
                {
                    p = _Network[i];
                    dist = p[1] - g; /* inx key */
                    if (dist >= bestd)
                        i = _Netsize; /* stop iter */
                    else
                    {
                        i++;
                        if (dist < 0)
                            dist = -dist;
                        a = p[0] - b;
                        if (a < 0)
                            a = -a;
                        dist += a;
                        if (dist < bestd)
                        {
                            a = p[2] - r;
                            if (a < 0)
                                a = -a;
                            dist += a;
                            if (dist < bestd)
                            {
                                bestd = dist;
                                best = p[3];
                            }
                        }
                    }
                }
                if (j >= 0)
                {
                    p = _Network[j];
                    dist = g - p[1]; /* inx key - reverse dif */
                    if (dist >= bestd)
                        j = -1; /* stop iter */
                    else
                    {
                        j--;
                        if (dist < 0)
                            dist = -dist;
                        a = p[0] - b;
                        if (a < 0)
                            a = -a;
                        dist += a;
                        if (dist < bestd)
                        {
                            a = p[2] - r;
                            if (a < 0)
                                a = -a;
                            dist += a;
                            if (dist < bestd)
                            {
                                bestd = dist;
                                best = p[3];
                            }
                        }
                    }
                }
            }
            return (best);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] Process()
        {
            Learn();
            Unbiasnet();
            Inxbuild();
            return ColorMap();
        }

        /// <summary>
        /// Unbias network to give byte values 0..255 and record position i to prepare for sort
        /// </summary>
        public void Unbiasnet()
        {
            for (int i = 0; i < _Netsize; i++)
            {
                _Network[i][0] >>= _NetBiasShift;
                _Network[i][1] >>= _NetBiasShift;
                _Network[i][2] >>= _NetBiasShift;
                _Network[i][3] = i; /* record colour no */
            }
        }

        /// <summary>
        /// Move adjacent neurons by precomputed alpha*(1-((i-j)^2/[r]^2)) in radpower[|i-j|]
        /// </summary>
        /// <param name="rad"></param>
        /// <param name="i"></param>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="r"></param>
        protected void Alterneigh(int rad, int i, int b, int g, int r)
        {

            int j = i + 1, k = i - 1, lo = i - rad, hi = i + rad, a, m = 1;
            int[] p;
            if (lo < -1)
                lo = -1;
            if (hi > _Netsize)
                hi = _Netsize;
            while ((j < hi) || (k > lo))
            {
                a = _RadiusPower[m++];
                if (j < hi)
                {
                    p = _Network[j++];
                    try
                    {
                        p[0] -= (a * (p[0] - b)) / _AlphaRadiusBias2;
                        p[1] -= (a * (p[1] - g)) / _AlphaRadiusBias2;
                        p[2] -= (a * (p[2] - r)) / _AlphaRadiusBias2;
                    }
                    catch (Exception)
                    {
                    } // prevents 1.3 miscompilation
                }
                if (k > lo)
                {
                    p = _Network[k--];
                    try
                    {
                        p[0] -= (a * (p[0] - b)) / _AlphaRadiusBias2;
                        p[1] -= (a * (p[1] - g)) / _AlphaRadiusBias2;
                        p[2] -= (a * (p[2] - r)) / _AlphaRadiusBias2;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Move neuron i towards biased (b,g,r) by factor alpha
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="i"></param>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="r"></param>
        protected void Altersingle(int alpha, int i, int b, int g, int r)
        {
            /* alter hit neuron */
            int[] n = _Network[i];
            n[0] -= (alpha * (n[0] - b)) / _InitAlpha;
            n[1] -= (alpha * (n[1] - g)) / _InitAlpha;
            n[2] -= (alpha * (n[2] - r)) / _InitAlpha;
        }

        /// <summary>
        /// Search for biased BGR values
        /// </summary>
        /// <param name="b"></param>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        protected int Contest(int b, int g, int r)
        {
            /* finds closest neuron (min dist) and updates freq */
            /* finds best neuron (min dist-bias) and returns position */
            /* for frequently chosen neurons, freq[i] is high and bias[i] is negative */
            /* bias[i] = gamma*((1/netsize)-freq[i]) */
            int i, dist, a, biasdist, betafreq;
            int bestpos, bestbiaspos, bestd, bestbiasd;
            int[] n;
            bestd = ~(((int)1) << 31);
            bestbiasd = bestd;
            bestpos = -1;
            bestbiaspos = bestpos;
            for (i = 0; i < _Netsize; i++)
            {
                n = _Network[i];
                dist = n[0] - b;
                if (dist < 0)
                    dist = -dist;
                a = n[1] - g;
                if (a < 0)
                    a = -a;
                dist += a;
                a = n[2] - r;
                if (a < 0)
                    a = -a;
                dist += a;
                if (dist < bestd)
                {
                    bestd = dist;
                    bestpos = i;
                }
                biasdist = dist - (_Bias[i] >> (_IntBiasShift - _NetBiasShift));
                if (biasdist < bestbiasd)
                {
                    bestbiasd = biasdist;
                    bestbiaspos = i;
                }
                betafreq = (_Freq[i] >> _BetaShift);
                _Freq[i] -= betafreq;
                _Bias[i] += (betafreq << _GammaShift);
            }
            _Freq[bestpos] += _Beta;
            _Bias[bestpos] -= _BetaGamma;
            return (bestbiaspos);
        }
    }
}