using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuYu.Components
{
    /// <summary>
    /// 分页信息类
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// 创建一个分页信息实例
        /// </summary>
        public Pager() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataCount">数据量</param>
        /// <param name="currNo">当前页码</param>
        /// <param name="pageSize">每页显示数据量</param>
        public Pager(int dataCount, int currNo, int pageSize)
        {
            this._DataCount = dataCount;
            this._CurrNo = currNo;
            this._PageSize = pageSize;
        }

        private int _DataCount;
        private int _CurrNo;
        private int _PageSize;

        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount
        {
            get { return _DataCount > 0 ? _DataCount : 0; }
            set { _DataCount = value; }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrNo
        {
            get
            {
                if (_CurrNo > 0 && _CurrNo <= PageCount)
                    return _CurrNo;
                else if (_CurrNo < 1)
                    return 1;
                else if (_CurrNo > PageCount)
                    return PageCount;
                else
                    return 1;
            }
            set { _CurrNo = value; }
        }

        /// <summary>
        /// 每页显示数据量
        /// </summary>
        public int PageSize
        {
            get { return _PageSize > 0 ? _PageSize : 10; }
            set { _PageSize = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return Math.Ceiling((double)DataCount / PageSize) < 1 ? 1 : (int)Math.Ceiling((double)DataCount / PageSize); }
        }
    }
}