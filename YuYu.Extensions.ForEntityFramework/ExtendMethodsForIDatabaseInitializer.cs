using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethodsForIDatabaseInitializer
    {
        /// <summary>
        /// 执行自定义约束
        /// 在 IDatabaseInitializer 的派生类的 Seed 方法最开始调用此扩展方法以在生成数据库时自动添加自定义约束
        /// 仅针对SqlServer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseInitializer"></param>
        /// <param name="dbContext"></param>
        /// <param name="pluralizingTableNameConventionEnabled"></param>
        public static void ExcuteCustomConstraint<T>(this IDatabaseInitializer<T> databaseInitializer, T dbContext, bool pluralizingTableNameConventionEnabled = true) where T : DbContext
        {
            IList<string> basePropertyNames = typeof(T).GetProperties().Select(m => m.Name).ToList();
            
            //先剔除由父类继承的公共属性
            foreach (PropertyInfo table in typeof(T).GetProperties().Where(m => basePropertyNames.IndexOf(m.Name) < 0).Select(m => m))
            {
                //获取子类DbSet<T>中的那个"T"
                Type modelType = table.PropertyType.GetGenericArguments()[0];
                string tableName = _GetTableName(modelType, pluralizingTableNameConventionEnabled);

                //逐个开始用SQL命令生成默认值约束
                foreach (PropertyInfo column in _GetColumns<DefaultValueAttribute>(modelType))
                {
                    DefaultValueAttribute defaultValueAttribute = column.GetCustomAttributes(typeof(DefaultValueAttribute), true).FirstOrDefault() as DefaultValueAttribute;
                    dbContext.Database.ExecuteSqlCommand("ALTER TABLE [" + tableName + "] ADD DEFAULT(" + defaultValueAttribute.Value + ") FOR [" + _GetColumnName(column) + "]");
                }

                //逐个开始用SQL命令生成检查约束
                foreach (PropertyInfo column in _GetColumns<CheckAttribute>(modelType))
                {
                    CheckAttribute checkAttribute = column.GetCustomAttributes(typeof(CheckAttribute), true).FirstOrDefault() as CheckAttribute;
                    dbContext.Database.ExecuteSqlCommand("ALTER TABLE [" + tableName + "] ADD CHECK(" + checkAttribute.Expression.Replace("{0}", _GetColumnName(column)) + ")");
                }

                //逐个开始用SQL命令生成唯一索引
                foreach (PropertyInfo column in _GetColumns<UniqueAttribute>(modelType))
                {
                    dbContext.Database.ExecuteSqlCommand("ALTER TABLE [" + tableName + "] ADD UNIQUE([" + _GetColumnName(column) + "])");
                }

                //逐个开始用SQL命令生成非聚集索引
                foreach (PropertyInfo column in _GetColumns<IndexAttribute>(modelType))
                {
                    dbContext.Database.ExecuteSqlCommand("CREATE INDEX [IX_" + _GetColumnName(column) + "_" + Guid.NewGuid().ToString("N").ToUpper() + "] ON [" + tableName + "]([" + _GetColumnName(column) + "])");
                }
            }
        }

        private static EnglishPluralizationService _EnglishPluralizationService;

        private static string _GetTableName(Type modelType, bool pluralizingTableNameConventionEnabled)
        {
            string tableName = string.Empty;
            if (modelType.GetCustomAttributes(typeof(TableAttribute), true).Count() > 0)
            {
                TableAttribute tableAttribute = modelType.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
                tableName = tableAttribute.Name;
            }
            else if (pluralizingTableNameConventionEnabled)
            {
                if (_EnglishPluralizationService == null)
                    _EnglishPluralizationService = new EnglishPluralizationService();
                tableName =  _EnglishPluralizationService.Pluralize(modelType.Name);
            }
            else
                tableName = modelType.Name;
            return tableName;
        }

        private static string _GetColumnName(PropertyInfo propertyInfo)
        {
            ColumnAttribute columnAttribute = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
            return columnAttribute == null ? propertyInfo.Name : columnAttribute.Name;
        }

        private static IList<PropertyInfo> _GetColumns<TAttribute>(Type modelType)
        {
            return modelType.GetProperties()
                .Where(m => m.GetCustomAttributes(typeof(TAttribute), true).Count() > 0)
                .ToList();
        }
    }
}
