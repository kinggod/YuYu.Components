using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForIEnumerable
    {
        /// <summary>
        /// 转换为 IEnumerable&lt;SelectListItem&gt; 类型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="getDisplay">获取显示的文本</param>
        /// <param name="getValue">获取表单提交值</param>
        /// <param name="selectedValue">已被选择的项</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> source, Func<T, string> getDisplay, Func<T, string> getValue, string selectedValue = null)
        {
            IList<SelectListItem> selectListItems = new List<SelectListItem>(source.Count());
            if (source != null)
                foreach (T item in source)
                {
                    string value = getValue(item);
                    selectListItems.Add(new SelectListItem()
                    {
                        Selected = value.Equals(selectedValue),
                        Text = getDisplay(item),
                        Value = value,
                    });
                }
            return selectListItems;
        }
    }
}
