using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MyChy.Frame.Common.Data
{
    public sealed class PagerWhereEntity
    {
        public PagerWhereEntity(Database db)
        {
            Db = db;
            WhereBuilder = new StringBuilder();
            List = new List<SqlParameter>();
            Pageid = 1;
            Pagesize = 20;
        }

        /// <summary>
        /// 显示列
        /// </summary>
        public string QueryField { get; set; }

        /// <summary>
        /// 主键列
        /// </summary>
        public string KeyField { get; set; }

        

        /// <summary>
        /// 排序列
        /// </summary>
        public string OrderField { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StringBuilder WhereBuilder { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public SqlParameter[] Parms { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<SqlParameter> List { get; set; }

        public int Pageid { get; set; }

        public int Pagesize { get; set; }

        public int RowsCount { get; set; }

        Database Db { get; set; }

        public PagerDataResult<T> PagerDataResult<T>()
        {
            if (List != null && List.Count > 0)
            {
                Parms = List.ToArray();
            }
            if (WhereBuilder.Length > 0 && string.IsNullOrEmpty(Where))
            {
                Where = WhereBuilder.ToString();
            }
        

            if (string.IsNullOrEmpty(QueryField)) QueryField = "*";
            if (string.IsNullOrEmpty(OrderField))
            {
                OrderField = "id";
            }
            if (string.IsNullOrEmpty(KeyField)) KeyField = OrderField;
            var sqlpager = new PagerSqlPager
            {
                Db = Db,
                KeyField = KeyField,
                OrderField = OrderField,
                PageIndex = Pageid,
                PageSize = Pagesize,
                Parms = Parms,
                QueryField = QueryField,
                TableName = Table
            };
            sqlpager.StrWhere.Append(Where);
            return sqlpager.GetDataResult<T>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PagerDataResult<T>
    {
        public PagerDataResult()
        {
            PagerData = new List<T>(0);
            PagerTable = null;
        }

        /// <summary>
        /// 数据内容 类
        /// </summary>
        public IList<T> PagerData { get; set; }

        /// <summary>
        ///  数据内容 表
        /// </summary>
        public DataTable PagerTable { get; set; }

        public int Pageid { get; set; }

        public int Pagesize { get; set; }

        public int RowsCount { get; set; }

    }


    internal sealed class PagerSqlPager
    {
        #region 私有属性

        private string _desc = "";

        private int _pageIndex = 1;

        #endregion


        #region 共有属性

        /// <summary>
        /// 顺序 倒序 默认顺序
        /// </summary>
        public bool OrderType { get; set; } = false;


        /// <summary>
        /// 主键列
        /// </summary>
        public string KeyField { get; set; } = "id";

        /// <summary>
        /// 排序列
        /// </summary>
        public string OrderField { get; set; } = "id";

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }

            set
            {
                _pageIndex = value;
                if (_pageIndex < 1)
                {
                    _pageIndex = 1;
                }
            }
        }

        /// <summary>
        /// 每页显示数据数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 数据总行数
        /// </summary>
        public int RowsCount { get; set; } = 0;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 显示行名
        /// </summary>
        public string QueryField { get; set; } = "*";


        /// <summary>
        /// where 必须以 
        /// </summary>
        public StringBuilder StrWhere { get; set; } = new StringBuilder();

        public SqlParameter[] Parms { get; set; } = null;

        /// <summary>
        /// 数据库
        /// </summary>
        public Database Db { get; set; }


        #endregion


        #region 共有方法

        /// <summary>
        /// 返回分页表
        /// </summary>
        /// <returns></returns>
        public PagerDataResult<T> GetDataResult<T>()
        {
            var resule = new PagerDataResult<T> {PagerTable = GetPagerTable()};
            if (resule.PagerTable == null) return resule;
            resule.Pageid = _pageIndex;
            resule.Pagesize = PageSize;
            resule.RowsCount = RowsCount;
            resule.PagerData = ModelHelper.GetListModelByTable<T>(resule.PagerTable);
            return resule;
        }

        /// <summary>
        /// 返回分页表
        /// </summary>
        /// <returns></returns>
        public DataTable GetPagerTable()
        {
            if (RowsCount == 0)
            {
                GetRowCount();
            }
            return RowsCount > 0 ? GetTable() : null;
        }

        #endregion



        #region 私有方法


        private DataTable GetTable()
        {
            if (!this.OrderType)
            {
                this._desc = "desc";
            }
            if (OrderField == "id")
            {
                OrderField = KeyField;
            }

            return TableName.IndexOf("join", StringComparison.Ordinal) > 0 ? GetMySqlDataMore() : GetMySqlDataOne();
        }




        private DataTable GetMySqlDataOne()
        {
            var sb = new StringBuilder();

            if (PageSize < 1) { PageSize = 20; }
            if (_pageIndex < 1) { _pageIndex = 1; }

            var minid = (_pageIndex - 1) * PageSize;
            var maxid = _pageIndex * PageSize;

            if (QueryField == "*")
            {
                QueryField = "m.*";
                TableName = TableName + " m ";
            }
            sb.AppendFormat("select * from");
            sb.AppendFormat(" (SELECT ROW_NUMBER() OVER(ORDER BY {0} {1}) AS _ROWID,{2} as _id",
                this.OrderField, _desc, KeyField);

            sb.AppendFormat(",{0} FROM {1} ", QueryField, this.TableName);
            sb.AppendFormat(" where 1=1 {0} ) as _tab ", StrWhere);
            sb.AppendFormat(" where _tab._rowid>@minid and _tab._rowid<=@maxid ");

            sb.Append(" order by _tab._rowid ");


            SqlParameter[] parmeter = {
                                new SqlParameter("@minid",(_pageIndex - 1) * PageSize),
                                new SqlParameter("@maxid",_pageIndex * PageSize),
            };

            return GetDataParms(sb.ToString(), parmeter);
        }

        private DataTable GetMySqlDataMore()
        {
            var sb = new StringBuilder();
            sb.Append("select * from (");
            sb.AppendFormat(" SELECT ROW_NUMBER() OVER(ORDER BY {0} {1}) AS _ROWID,", this.OrderField, _desc);
            sb.AppendFormat(" {0}", QueryField);
            sb.AppendFormat(" from {0}", this.TableName);
            sb.AppendFormat(" where {0}", StrWhere.ToString());
            sb.Append(" ) as _tab");
            sb.Append(" where _rowid>@minid and _rowid<=@maxid");
            sb.Append(" order by _tab._ROWID");

            SqlParameter[] parmeter = {
                                new SqlParameter("@minid", (_pageIndex - 1) * PageSize),
                                new SqlParameter("@maxid",_pageIndex * PageSize),
            };

            return GetDataParms(sb.ToString(), parmeter);
        }


        private DataTable GetDataParms(string sqltxt, SqlParameter[] parms)
        {
            SqlParameter[] allparms = null;
            if (Parms != null)
            {
                var ilist = new List<SqlParameter>(parms.Length + Parms.Length);
                ilist.AddRange(parms.Select(p => (SqlParameter) ((ICloneable) p).Clone()));
                ilist.AddRange(Parms.Select(p => (SqlParameter) ((ICloneable) p).Clone()));
                allparms = ilist.ToArray();
            }
            else
            {
                allparms = parms;
            }
            return DataHelperBase.GetDataTable(Db, sqltxt, allparms);
        }


        private void GetRowCount()
        {
            var sqltxt = " select count(1) from " + TableName + " where 1=1 " + StrWhere.ToString();
            RowsCount = DataHelperBase.ExecuteScalar<int>(this.Db, sqltxt, this.Parms, 0);
        }




        #endregion

    }

}
