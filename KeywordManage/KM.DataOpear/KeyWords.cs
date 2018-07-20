using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KM.DBUtility;//Please add references
using KM.Common;
using System.IO;
using System.Collections.Generic;

namespace KM.DataOpear
{
    /// <summary>
    /// 数据访问类:KeyWords
    /// </summary>
    public partial class KeyWords
    {
        public KeyWords()
        {

        }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string KID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from KeyWords");
            strSql.Append(" where KID=@KID ");
            SqlParameter[] parameters = 
            {
				new SqlParameter("@KID", SqlDbType.NVarChar,30)			
            };
            parameters[0].Value = KID;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(KM.Entity.KeyWords model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into KeyWords(");
            strSql.Append("KID,KeyWordsName,KeyWordsStatus)");
            strSql.Append(" values (");
            strSql.Append("@KID,@KeyWordsName,@KeyWordsStatus)");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@KID", SqlDbType.NVarChar,30),
					new SqlParameter("@KeyWordsName", SqlDbType.NVarChar,200),
					new SqlParameter("@KeyWordsStatus", SqlDbType.NVarChar,10)
            };
            parameters[0].Value = model.KID;
            parameters[1].Value = model.KeyWordsName;
            parameters[2].Value = model.KeyWordsStatus;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(KM.Entity.KeyWords model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update KeyWords set ");
            strSql.Append("KeyWordsName=@KeyWordsName,");
            strSql.Append("KeyWordsStatus=@KeyWordsStatus");
            strSql.Append(" where KID=@KID ");
            SqlParameter[] parameters = 
            {
				new SqlParameter("@KeyWordsName", SqlDbType.NVarChar,200),
				new SqlParameter("@KeyWordsStatus", SqlDbType.NVarChar,10),
				new SqlParameter("@KID", SqlDbType.NVarChar,30)
            };
            parameters[0].Value = model.KeyWordsName;
            parameters[1].Value = model.KeyWordsStatus;
            parameters[2].Value = model.KID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string KID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from KeyWords ");
            strSql.Append(" where KID=@KID ");
            SqlParameter[] parameters = 
            {
				new SqlParameter("@KID", SqlDbType.NVarChar,30)			
            };
            parameters[0].Value = KID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string KIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from KeyWords ");
            strSql.Append(" where KID in (" + KIDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KM.Entity.KeyWords GetModel(string KID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 KID,KeyWordsName,KeyWordsStatus from KeyWords ");
            strSql.Append(" where KID=@KID ");
            SqlParameter[] parameters = 
            {
				new SqlParameter("@KID", SqlDbType.NVarChar,30)			
            };
            parameters[0].Value = KID;

            KM.Entity.KeyWords model = new KM.Entity.KeyWords();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KM.Entity.KeyWords DataRowToModel(DataRow row)
        {
            KM.Entity.KeyWords model = new KM.Entity.KeyWords();
            if (row != null)
            {
                if (row["KID"] != null)
                {
                    model.KID = row["KID"].ToString();
                }
                if (row["KeyWordsName"] != null)
                {
                    model.KeyWordsName = row["KeyWordsName"].ToString();
                }
                if (row["KeyWordsStatus"] != null)
                {
                    model.KeyWordsStatus = row["KeyWordsStatus"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select KID,KeyWordsName,KeyWordsStatus ");
            strSql.Append(" FROM KeyWords ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" KID,KeyWordsName,KeyWordsStatus ");
            strSql.Append(" FROM KeyWords ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM KeyWords ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.KID desc");
            }
            strSql.Append(")AS Row, T.*  from KeyWords T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "KeyWords";
            parameters[1].Value = "KID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

        #region  ExtensionMethod
        /// <summary>
        /// 获得分页的数据
        /// </summary>
        /// <param name="pageSize">每页的条数</param>
        /// <param name="pageIndex">显示的第几页</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="fieldOrder">排序字段【不可为空】</param>
        /// <param name="recordCount">返回记录的总条数</param>
        /// <returns></returns>
        public DataTable GetListByPage(int pageSize, int pageIndex, string strWhere, string fieldOrder, out int recordCount)
        {
            StringBuilder sbr = new StringBuilder();
            sbr.Append("select KID as 关键字ID,KeyWordsName 关键字 ,KeyWordsStatus 状态 FROM KeyWords");
            if (strWhere.Trim() != "")
            {
                sbr.Append("  where " + strWhere);
            }

            //统计记录的SQL语句
            string strCountSQL = PagingHelper.CreateCountingSQL(sbr.ToString());

            //统计的记录数
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(strCountSQL));

            //分页的SQL语句
            string strPageSQL = PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, sbr.ToString(), fieldOrder);

            return DbHelperSQL.Query(strPageSQL).Tables[0];

        }

        //关键字的批量录入 支持txt 和excel文件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        public void ImporData(string filePath)
        {
            string strExt = Path.GetExtension(filePath).Trim();
            switch (strExt)
            {
                case ".txt":
                    List<string> alist = Tools.GetkeyWords(filePath);
                    for (int i = 0; i < alist.Count; i++)
                    {
                        KM.Entity.KeyWords kws = new Entity.KeyWords();
                        kws.KID = CommonSpace.Conmmon.GenerateStringID();
                        kws.KeyWordsName = alist[i];
                        kws.KeyWordsStatus = "yes";
                        Add(kws);
                    }

                    alist.Clear();
                    break;
                case ".xls":
                    break;
                case ".xlsx":
                    break;
                default:
                    break;
            }
        }




        #endregion  ExtensionMethod
    }
}

