using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MyChy.Frame.Common.Data
{
    /// <summary>
    /// OracleDataBase 的摘要说明
    /// </summary>
    public class DataHelperBase
    {
        /// <summary>
        /// 执行DbCommand 返回list类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static IList<T> GetListEntity<T>(Database db, DbCommand cmd)
        {
            using (var ds = db.ExecuteDataSet(cmd))
            {
                return ModelHelper.GetListModelByTable<T>(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 执行DbCommand 返回类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static T GetEntity<T>(Database db, DbCommand cmd)
        {
            using (var ds = db.ExecuteDataSet(cmd))
            {
                return ModelHelper.GetModelByTable<T>(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 执行sqltxt 返回list类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <returns></returns>
        public static IList<T> GetListEntity<T>(Database db, string sqltxt)
        {
            return GetListEntity<T>(db, sqltxt, null);
        }

        /// <summary>
        /// 执行sqltxt 返回类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <returns></returns>
        public static T GetEntity<T>(Database db, string sqltxt)
        {
            return GetEntity<T>(db, sqltxt, null);
        }

        /// <summary>
        /// 执行sqltxt 返回list类 带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static IList<T> GetListEntity<T>(Database db, string sqltxt, SqlParameter[] parms)
        {
            var da = GetDataTable(db, sqltxt, parms);
            return ModelHelper.GetListModelByTable<T>(da);
        }

        /// <summary>
        /// 执行sqltxt 返回类 带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T GetEntity<T>(Database db, string sqltxt, SqlParameter[] parms)
        {
            var da = GetDataTable(db, sqltxt, parms);
            return ModelHelper.GetModelByTable<T>(da);
        }

        /// <summary>
        /// 执行sqltxt 返回Table
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(Database db, string sqltxt, SqlParameter[] parms)
        {
            DataTable data = null;
            var set = GetDataSet(db, sqltxt, parms);
            if (set != null && set.Tables.Count > 0)
            {
                data = set.Tables[0];
            }
            return data;
        }

        /// <summary>
        /// 执行sqltxt 返回DataSet
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(Database db, string sqltxt, SqlParameter[] parms)
        {
            DataSet ds;
            using (var cmd = db.GetSqlStringCommand(sqltxt))
            {
                if (parms != null)
                {
                    foreach (var i in parms)
                    {
                        db.AddInParameter(cmd, i.ParameterName, i.DbType, i.Value);
                    }
                }
                ds = db.ExecuteDataSet(cmd);
            }
            return ds;
        }

        /// <summary>
        ///  执行sqltxt 返回Scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(Database db, string sqltxt, T def)
        {
            return ExecuteScalar<T>(db, sqltxt, null, def);
        }

        /// <summary>
        /// 执行sqltxt 返回Scalar 返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="parms"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(Database db, string sqltxt, SqlParameter[] parms, T def)
        {

            object obj = null;
            using (var cmd = db.GetSqlStringCommand(sqltxt))
            {
                if (parms != null)
                {
                    foreach (var i in parms)
                    {
                        if (i.Value != null)
                        {
                            db.AddInParameter(cmd, i.ParameterName, i.DbType, i.Value);
                        }
                    }
                }
                obj = db.ExecuteScalar(cmd);
            }
            return obj.To<T>(def);
        }

        /// <summary>
        /// 执行sqltxt 返回NonQuery
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(Database db, string sqltxt)
        {
            return ExecuteNonQuery(db, sqltxt, null);
        }

        /// <summary>
        /// 执行sqltxt 返回NonQuery  带参数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqltxt"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(Database db, string sqltxt, SqlParameter[] parms)
        {
            using (var cmd = db.GetSqlStringCommand(sqltxt))
            {
                if (parms == null) return db.ExecuteNonQuery(cmd);
                foreach (var i in parms)
                {
                    if (i.Value != null)
                    {
                        db.AddInParameter(cmd, i.ParameterName, i.DbType, i.Value);
                    }
                }
                return db.ExecuteNonQuery(cmd);
            }
        }
    }
}
