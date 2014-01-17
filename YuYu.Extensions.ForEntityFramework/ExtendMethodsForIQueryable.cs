using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 对 IQueryable 进行的扩展
    /// </summary>
    public static class ExtendMethodsForIQueryable
    {
        /// <summary>
        /// 具有 TEntity 类型输入参数并无返回值的方法委托
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity"></param>
        public delegate void Func<TEntity>(TEntity entity);

        /// <summary>
        /// 将集合中满足条件的所有实体置于 pending delete 状态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="dbSet">实体集合</param>
        /// <param name="predicate">检索条件</param>
        public static void Remove<TEntity>(this IDbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            IList<TEntity> entities = dbSet
                .Where(predicate)
                .ToList();
            for (int i = 0; i < entities.Count; i++)
                dbSet.Remove(entities[i]);
        }

        /// <summary>
        /// 通过自定义方法将集合中满足条件的所有实体置于更新状态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entitySet">实体集合</param>
        /// <param name="predicate">检索条件</param>
        /// <param name="update">更新方法</param>
        public static void Update<TEntity>(this IQueryable<TEntity> entitySet, Expression<Func<TEntity, bool>> predicate, Func<TEntity> update) where TEntity : class
        {
            IList<TEntity> entities = entitySet
                .Where(predicate)
                .ToList();
            for (int i = 0; i < entities.Count; i++)
                update(entities[i]);
        }

        /// <summary>
        /// 随机查询定量数据行
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitySet"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IList<TEntity> RandomSelect<TEntity>(this IOrderedQueryable<TEntity> entitySet, int count)
        {
            int totalCount = entitySet.Count();
            Random random = new Random();
            if (totalCount > 1)
            {
                int seed = totalCount > count ? count : totalCount;
                IList<int> skipCounts = new List<int>(seed);
                IList<TEntity> results = new List<TEntity>(seed);
                for (int i = 0; i < count; i++)
                {
                    int skipCount = random.Next(seed);
                    while (skipCounts.Contains(skipCount))
                        skipCount = random.Next(seed);
                    results.Add(entitySet.Skip(skipCount).FirstOrDefault());
                }
                return results;
            }
            else
                return entitySet.ToList();
        }
    }
}
