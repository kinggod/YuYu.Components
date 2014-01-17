using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// IEnumerable 类型的扩展方法
    /// </summary>
    public static class ExtendMethodsForIEnumerable
    {
        /// <summary>
        /// 判断此集合为null或空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }

        /// <summary>
        /// 判断此集合为非空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return !IsNullOrEmpty<T>(source);
        }

        /// <summary>
        /// 以指定的字符拼接字符串可枚举集合中的每一个元素
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separater">用来拼接元素的字符串</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source, char separater = ',')
        {
            return Join(source, separater.ToString());
        }

        /// <summary>
        /// 以指定的字符串拼接字符串可枚举集合中的每一个元素
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separater">用来拼接元素的字符串</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source, string separater)
        {
            if (source == null || source.Count() == 0)
                return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (string str in source)
            {
                result.Append(str);
                result.Append(separater);
            }
            result.Remove(result.Length - separater.Length, separater.Length);
            return result.ToString();
        }

        /// <summary>
        /// 以指定的字符将int, long, float, double, decimal型可枚举集合中的元素拼接成字符串
        /// </summary>
        /// <typeparam name="T">int, long, float, double, decimal</typeparam>
        /// <param name="source"></param>
        /// <param name="separater">用来拼接元素的字符串</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, char separater = ',') where T : struct
        {
            return Join(source, separater.ToString());
        }

        /// <summary>
        /// 以指定的字符串将int, long, float, double, decimal型可枚举集合中的元素拼接成字符串
        /// </summary>
        /// <typeparam name="T">int, long, float, double, decimal</typeparam>
        /// <param name="source"></param>
        /// <param name="separater">用来拼接元素的字符串</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, string separater) where T : struct
        {
            if (source == null || source.Count() == 0)
                return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (T t in source)
            {
                result.Append(t.ToString());
                result.Append(separater);
            }
            result.Remove(result.Length - separater.Length, separater.Length);
            return result.ToString();
        }

        /// <summary>
        /// 以指定的字符拼接可枚举集合中的每一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separater">用来拼接元素的字符串</param>
        /// <param name="str">返回一个字符串</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, Func<T, string> str, char separater = ',')
        {
            return Join(source, str, separater.ToString());
        }

        /// <summary>
        /// 以指定的字符串拼接可枚举集合中的每一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separater">用来拼接元素的字符串</param>
        /// <param name="str">返回一个字符串</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> source, Func<T, string> str, string separater)
        {
            if (source == null || source.Count() == 0)
                return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (T t in source)
            {
                result.Append(str(t));
                result.Append(separater);
            }
            result.Remove(result.Length - separater.Length, separater.Length);
            return result.ToString();
        }

        /// <summary>
        /// 将 T1 类型集合数据转换成 T2 类型集合数据
        /// </summary>
        /// <typeparam name="T1">转换的目标类型</typeparam>
        /// <typeparam name="T2">转换的源类型</typeparam>
        /// <param name="source">待转换的数据集合</param>
        /// <param name="transfer">转换方法，输入 T1 类型，返回 T2 类型</param>
        /// <returns></returns>
        public static IList<T2> Cast<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> transfer)
        {
            IList<T2> list = new List<T2>();
            if (source != null && source.Count() >= 0)
            {
                foreach (T1 t in source)
                {
                    list.Add(transfer(t));
                }
            }
            return list;
        }

        /// <summary>
        /// 随机排序，将可枚举集合中的元素顺序随机打乱
        /// （此方法保留源集合及其排序状态）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">源集合数据</param>
        /// <param name="keepSource">保持元数据状态</param>
        /// <returns>随机排序后的集合数据</returns>
        public static ICollection<T> RandomSort<T>(this IEnumerable<T> source, bool keepSource = false)
        {
            int index = 0;
            IDictionary<int, T> tmp = source.ToDictionary((s) => { index++; return index - 1; }, s => s);
            IList<int> tmpIs = new List<int>(tmp.Count / 2 + 1);
            Random r = new Random();
            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmpIs.Contains(i))
                    continue;
                int tmpI = r.Next(tmp.Count);
                tmpIs.Add(tmpI);
                T t1 = tmp[i];
                T t2 = tmp[tmpI];
                tmp[i] = t2;
                tmp[tmpI] = t1;
            }
            tmpIs.Clear();
            tmpIs = null;
            r = null;
            if (!keepSource)
                source = null;
            return tmp.Values;
        }

        /// <summary>
        /// 从集合中随机抽出定量元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IList<T> RandomSelect<T>(this IOrderedEnumerable<T> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentException("The value of parameter “count” must greater than 0!");
            int totalCount = source.Count();
            if (totalCount > count)
            {
                Random r = new Random();
                IList<int> skips = new List<int>(count);
                IList<T> results = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    int skip = r.Next(totalCount);
                    while (skips.Contains(skip))
                    {
                        skip = r.Next(totalCount);
                    }
                    results.Add(source.Skip(skip).FirstOrDefault());
                }
                return results;
            }
            else
                return source.Take(count).ToList();
        }
    }
}
