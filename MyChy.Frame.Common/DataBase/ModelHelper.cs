﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.DataBase
{
    /// <summary>
    /// ModelHelper 的摘要说明
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// table 自动转换成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="da"></param>
        /// <returns></returns>
        public static T GetModelByTable<T>(DataTable da)
        {
            if (da.Rows.Count == 0) return default(T);

            var result = GetListModelByTable<T>(da);
            if (result != null && result.Count > 0)
            {
                return result.ToList<T>()[0];
            }
            return default(T);
        }


        /// <summary>
        /// table 自动转换成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="da"></param>
        /// <returns></returns>
        public static IList<T> GetListModelByTable<T>(DataTable da)
        {
            IList<T> result = new List<T>();
            if (da == null)
            {
                return result;
            }

            var t = typeof(T);
            var model = Activator.CreateInstance(t);
            var col = TypeDescriptor.GetProperties(model);
            var list = WebCache.GetCache<HashSet<string>>(t.FullName);
            if (list == null)
            {
                list = new HashSet<string>();
                foreach (PropertyDescriptor item in col)
                {
                    if (da.Columns.Contains(item.Name))
                    {
                        list.Add(item.Name);
                    }
                }
                WebCache.SetCache(t.FullName, list, 60);

            }


            foreach (DataRow dr in da.Rows)
            {
                model = Activator.CreateInstance(t);
                foreach (PropertyDescriptor item in col)
                {
                    if (!list.Contains(item.Name)) continue;
                    var value = ObjectExtension.GetValueByType(item.PropertyType, dr[item.Name]);
                    if (value != null)
                    {
                        item.SetValue(model, value);
                    }
                }
                if (model != null)
                {
                    result.Add((T)model);
                }
            }

            return result;
        }

        /// <summary>
        /// 类 自动转换成 table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable GetTableByListModel<T>(IList<T> list)
        {
            var result = new DataTable();
            if (list == null)
            {
                return result;
            }
            var t = typeof(T);
            var pi = t.GetProperties();
            foreach (PropertyInfo item in pi)
            {
                result.Columns.Add(new DataColumn(item.Name, item.PropertyType));
            }
            foreach (var i in list)
            {
                var newRow = result.NewRow();
                for (var j = 0; j < result.Columns.Count; j++)
                {
                    newRow[j] = t.InvokeMember(result.Columns[j].ColumnName, BindingFlags.GetProperty,
                        null, i, new object[] { });
                }
                result.Rows.Add(newRow);
            }
            return result;
        }
    }
}