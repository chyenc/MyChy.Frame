using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MyChy.Frame.Common;
using MyChy.Frame.Common.Data;

namespace MyChy.Frame.Test
{
    public class SqlTest
    {
        public class Wehy10000Sql
        {
            protected Database _db;

            public Wehy10000Sql()
            {
               // _db = DatabaseFactory.CreateDatabase("ConnectionHy10000");
                var factory = new DatabaseProviderFactory();
                _db = factory.Create("ConnectionHy10000");
            }

            public int Run(string openid)
            {
                const string sqlStr = @"SELECT count(1)
            FROM [CRM_Customer](nolock) 
            where openid=@openid;";
                var cmd = _db.GetSqlStringCommand(sqlStr);
                _db.AddInParameter(cmd, "@openid", DbType.String, openid);
                return _db.ExecuteScalar(cmd).To<int>();
            }
        }
    }
}
