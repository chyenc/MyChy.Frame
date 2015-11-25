using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MyChy.Frame.Common.DataBase
{
    /// <summary>
    /// OracleDataBase 的摘要说明
    /// </summary>
    public class DataHelperBase
    {
        public static IList<T> GetListEntity<T>(Database db, DbCommand cmd)
        {
            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                return ModelHelper.GetListModelByTable<T>(ds.Tables[0]);
            }
        }

        public static T GetEntity<T>(Database db, DbCommand cmd)
        {
            using (DataSet ds = db.ExecuteDataSet(cmd))
            {
                return ModelHelper.GetModelByTable<T>(ds.Tables[0]);
            }
        }


        public static IList<T> GetListEntity<T>(Database db, string Sqltxt)
        {
            return GetListEntity<T>(db, Sqltxt, null);
        }

        public static T GetEntity<T>(Database db, string Sqltxt)
        {
            return GetEntity<T>(db, Sqltxt, null);
        }

        public static IList<T> GetListEntity<T>(Database db, string Sqltxt, SqlParameter[] parms)
        {
            DataTable da = GetDataTable(db, Sqltxt, parms);
            return ModelHelper.GetListModelByTable<T>(da);
        }

        public static T GetEntity<T>(Database db, string Sqltxt, SqlParameter[] parms)
        {
            DataTable da = GetDataTable(db, Sqltxt, parms);
            return ModelHelper.GetModelByTable<T>(da);
        }

        public static DataTable GetDataTable(Database Db, string Sqltxt, SqlParameter[] parms)
        {
            DataTable data = null;
            DataSet Set = GetDataSet(Db, Sqltxt, parms);
            if (Set != null && Set.Tables.Count > 0)
            {
                data = Set.Tables[0];
            }
            return data;
        }

        public static DataSet GetDataSet(Database Db, string Sqltxt, SqlParameter[] parms)
        {
            DataSet ds = new DataSet();
            using (DbCommand cmd = Db.GetSqlStringCommand(Sqltxt))
            {
                if (parms != null)
                {
                    foreach (var i in parms)
                    {
                        Db.AddInParameter(cmd, i.ParameterName, i.DbType, i.Value);
                    }
                }
                ds = Db.ExecuteDataSet(cmd);
            }
            return ds;
        }


        public static T ExecuteScalar<T>(Database Db, string Sqltxt, T def)
        {
            return ExecuteScalar<T>(Db, Sqltxt, null, def);
        }

        public static T ExecuteScalar<T>(Database Db, string Sqltxt, SqlParameter[] parms, T def)
        {

            object obj = null;
            using (DbCommand cmd = Db.GetSqlStringCommand(Sqltxt))
            {
                if (parms != null)
                {
                    foreach (var i in parms)
                    {
                        if (i.Value != null)
                        {
                            Db.AddInParameter(cmd, i.ParameterName, i.DbType, i.Value);
                        }
                    }
                }
                obj = Db.ExecuteScalar(cmd);
            }
            return obj.To<T>(def);
        }

        public static int ExecuteNonQuery(Database Db, string Sqltxt)
        {
            return ExecuteNonQuery(Db, Sqltxt, null);
        }

        public static int ExecuteNonQuery(Database Db, string Sqltxt, SqlParameter[] parms)
        {
            using (DbCommand cmd = Db.GetSqlStringCommand(Sqltxt))
            {
                if (parms != null)
                {
                    foreach (var i in parms)
                    {
                        if (i.Value != null)
                        {
                            Db.AddInParameter(cmd, i.ParameterName, i.DbType, i.Value);
                        }
                    }
                }
                return Db.ExecuteNonQuery(cmd);
            }
        }
    }
}
