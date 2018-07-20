// ================================================================================
// 		File: ExcelFile.cs
// 		Desc: Excel操作类。
//            主要功能是用于Excel数据的导入导出。
//
// 		Called by:   
//               
// 		Auth: 汪洋
// 		Date: 2010年8月20日
// ================================================================================
// 		Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//
// ================================================================================
// Copyright (C) 2010-2012 
// ================================================================================
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace xiaoy.Excel
{
    /// <summary>
    /// Excel文件操作类。
    /// </summary>
    public class ExcelFile
    {
        #region 私有变量

        //excel版本，默认为98-2003版Excel。
        string m_version = string.Empty;

        //连接串中的HDR属性，用于标识第一行是列名。默认是。
        string m_hdr = string.Empty;

        //excel文件路径
        string m_filePath = string.Empty;

        //临时存放的数据表
        DataTable m_dataSource = null;

        //缓存静态变量
        static NameValueCollection m_excelVersion = null;
        static NameValueCollection m_excelTypeMap = null;

        //每个Sheel表存放的最大条数
        static int m_maxSheelSize = 0;

        #endregion

        #region 构造函数
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ExcelFile()
        {
            m_excelVersion = AppConfig.GetConfig(AppConfigKey.ExcelVersionKey);
            m_excelTypeMap = AppConfig.GetConfig(AppConfigKey.ExcelTypeKey);
            m_maxSheelSize = int.Parse(AppConfig.AppSettings[AppConfigKey.MaxSheelSize]);

            ////将m_excelVersion值中的'替换成"
            //foreach (string key in m_excelVersion.Keys)
            //{
            //    m_excelVersion[key] = m_excelVersion[key].Replace("'","\"");
            //}
        }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ExcelFile() : this(null) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="excelVersion">Excel文档版本，使用ExcelVersion类型。</param>
        public ExcelFile(string excelVersion)
        {
            //设置默认值
            m_version = ExcelVersion.Excel8;
            m_hdr = HDRType.Yes;


            //覆盖默认值
            if (!string.IsNullOrEmpty(excelVersion))
            {
                m_version = excelVersion;
            }
        }
        #endregion

        #region 公用属性
        /// <summary>
        /// 获取或设置Excel文档版本号。
        /// 为ExcelVersion类型的值。
        /// </summary>
        public string Version
        {
            get
            {
                return m_version;
            }
            set
            {
                m_version = value;
            }
        }

        /// <summary>
        /// 获取或设置HDR类型。
        /// </summary>
        public string HDR
        {
            get
            {
                return HDR;
            }
            set
            {
                HDR = value;
            }
        }

        /// <summary>
        /// 获取或设置Excel文件路径。
        /// </summary>
        public string FilePath
        {
            get
            {
                return m_filePath;
            }
            set
            {
                m_filePath = value;
            }
        }

        /// <summary>
        /// 获取或设置数据源。
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return m_dataSource;
            }
            set
            {
                m_dataSource = value;
            }
        }

        #endregion

        #region 公用方法
        /// <summary>
        /// 导入DataSource到Excel中
        /// </summary>
        public void Import()
        {
            SetData(DataSource, FilePath, Version, HDR);
        }

        /// <summary>
        /// 获取Excel中的数据
        /// </summary>
        /// <param name="bMerge">多表数据是否合并</param>
        /// <returns>DataTable集合</returns>
        public DataTable[] GetData(bool bMerge)
        {
            return GetData(FilePath, Version, HDR, bMerge);
        }
        #endregion

        #region 公用静态方法
        /// <summary>
        /// 写数据到Excel。
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="filePath">Excel导出路径</param>
        /// <param name="excelVersion">excel版本，为ExcelVersion类型</param>
        /// <param name="pHDR">第一行是否标题，为HDRType类型</param>
        public static void SetData(DataTable dtSource, string filePath, string excelVersion, string pHDR)
        {
            //数据源为空
            if (dtSource == null)
            {
                throw new Exception("无数据可导");
            }
            //保存路径为空
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("未设置Excel保存路径");
            }
            //删除文件
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //链接字符串
            string connectionString = string.Format(m_excelVersion[excelVersion], filePath, pHDR);

            // 连接Excel 
            using (OleDbConnection Connection = new OleDbConnection(connectionString))
            {
                Connection.Open();

                //导入数据  
                using (OleDbCommand command = new OleDbCommand())
                {
                    command.Connection = Connection;

                    //构建列  格式如：Name VarChar，CreateDate Date
                    string colList = CreateExcelColums(dtSource);

                    //构建插入SQL语句
                    //格式如 "INSERT INTO TABLE [tablename](col1,col2,col3) VALUES(@col1,@col2,@col3)"; 

                    StringBuilder sbColumNames = new StringBuilder();
                    StringBuilder sbColumValues = new StringBuilder();

                    foreach (DataColumn dc in dtSource.Columns)
                    {
                        sbColumNames.AppendFormat(",[{0}]", dc.ColumnName);
                        sbColumValues.AppendFormat(",@{0}", dc.ColumnName);
                    }

                    //去掉多余的逗号
                    sbColumNames.Remove(0, 1);
                    sbColumValues.Remove(0, 1);

                    //当数据量超过每页最大数据量时，自动分页
                    int totalRows = dtSource.Rows.Count;//总数据量
                    int pageIndex = 0;

                    //开始插入数据  do...while循环是为了处理分页逻辑
                    do
                    {
                        //计算此轮插入的数据量
                        int insertRows = m_maxSheelSize - 1;

                        //如果总数据量没有达到容量
                        if (totalRows < insertRows)
                        {
                            insertRows = totalRows;
                        }

                        string tableName = dtSource.TableName + pageIndex;
                        if (pageIndex == 0)
                        {
                            tableName = "Sheet1";
                        }

                        //创建表框架
                        StringBuilder sbCom = new StringBuilder();
                        sbCom.Append("CREATE TABLE [");
                        sbCom.Append(tableName);
                        sbCom.Append("](");
                        sbCom.Append(colList);
                        sbCom.Append(")");
                        command.CommandText = sbCom.ToString();

                        //try
                        //{
                        command.ExecuteNonQuery();
                        //}
                        //catch
                        //{
                        //    //如果使用Create语句创建失败则直接创建Excel文件
                        //    CreateExcelFile(filePath, excelVersion, command.CommandText);
                        //}

                        //插入数据
                        sbCom = new StringBuilder();
                        sbCom.AppendFormat("INSERT INTO [{0}]({1}) VALUES({2})",
                                            tableName, sbColumNames.ToString(), sbColumValues.ToString());

                        int startIndex = pageIndex * (m_maxSheelSize - 1);
                        int endIndex = pageIndex * (m_maxSheelSize - 1) + insertRows;

                        for (int i = startIndex; i < endIndex; i++)
                        {
                            DataRow drData = dtSource.Rows[i];
                            OleDbParameterCollection dbParam = command.Parameters;
                            dbParam.Clear();
                            foreach (DataColumn dc in dtSource.Columns)
                            {
                                dbParam.Add(new OleDbParameter("@" + dc.ColumnName, GetOleDbTypeByDataColumn(dc)));
                                dbParam["@" + dc.ColumnName].Value = drData[dc.ColumnName];
                            }
                            command.CommandText = sbCom.ToString();
                            command.ExecuteNonQuery();
                        }

                        //计算剩余数据量
                        totalRows = totalRows - insertRows;
                        pageIndex++;

                    } while (totalRows > 0);
                }//end of using OleDbCommand  
            }// end of  using OleDbConnection
        }

        /// <summary>
        /// 从Excel读数据
        /// </summary>
        /// <param name="filePath">excel文档路径</param>
        /// <param name="excelVersion">文档版本</param>
        /// <param name="pHDR">第一行是否标题</param>
        /// <param name="bMerge">
        /// 如果有多页，是否合并数据，合并时必须保证多页的表结构一致
        /// </param>
        /// <returns>DataTable集</returns>
        public static DataTable[] GetData(string filePath, string excelVersion, string pHDR, bool bMerge)
        {
            List<DataTable> dtResult = new List<DataTable>();

            //m_excelVersion这里面的值是合适被缓存起来的呢？ 什么使用写入的缓存呢？
            string connectionString = string.Format(m_excelVersion[excelVersion], filePath, pHDR);
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                string[] sheels = GetExcelWorkSheets(filePath, excelVersion);
                //GetExcelWorkSheets这个函数与本函数的关系：被调用与调用的关系，有个特点必须注意
                //GetExcelWorkSheets可以独立的使用  耦合度不太强
                foreach (string sheelName in sheels)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from [" + sheelName + "$]", con);

                        adapter.FillSchema(dtExcel, SchemaType.Mapped);
                        adapter.Fill(dtExcel);

                        dtExcel.TableName = sheelName;
                        dtResult.Add(dtExcel);

                        //虽然将一张excel中的多张数据表完全的读出来了 但是写出到新的excel中并没有按原来的进行
                    }
                    catch
                    {
                        //容错处理：取不到时，不报错，结果集为空即可。
                    }
                }

                //如果需要合并数据，则合并到第一张表
                if (bMerge)
                {
                    for (int i = 1; i < dtResult.Count; i++)
                    {
                        //如果不为空才合并
                        if (dtResult[0].Columns.Count == dtResult[i].Columns.Count &&
                            dtResult[i].Rows.Count > 0)
                        {
                            dtResult[0].Load(dtResult[i].CreateDataReader());
                        }
                    }
                }
            }
            //备注信息：数据不要追加到第一张表中而是要继续创建数据表 最好，这样能够保持和原来一样的结构
            return dtResult.ToArray();
        }

        #endregion

        #region 私有静态变量

        /// <summary>
        /// 返回指定文件所包含的工作簿列表;如果有WorkSheet，就返回以工作簿名字命名的ArrayList，否则返回空
        /// </summary>
        /// <param name="filePath">要获取的Excel</param>
        /// <param name="excelVersion">文档版本</param>
        /// <returns>如果有WorkSheet，就返回以工作簿名字命名的string[]，否则返回空</returns>
        private static string[] GetExcelWorkSheets(string filePath, string excelVersion)
        {
            List<string> alTables = new List<string>();
            string connectionString = string.Format(m_excelVersion[excelVersion],
              filePath, "Yes");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable dt = new DataTable();

                dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    throw new Exception("无法获取指定Excel的架构。");
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string tempName = dr["Table_Name"].ToString();

                    int iDolarIndex = tempName.IndexOf('$');

                    if (iDolarIndex > 0)
                    {
                        tempName = tempName.Substring(0, iDolarIndex);
                    }

                    //修正Excel2003中某些工作薄名称为汉字的表无法正确识别的BUG。
                    if (tempName[0] == '\'')
                    {
                        if (tempName[tempName.Length - 1] == '\'')
                        {
                            tempName = tempName.Substring(1, tempName.Length - 2);
                        }
                        else
                        {
                            tempName = tempName.Substring(1, tempName.Length - 1);
                        }

                    }
                    if (!alTables.Contains(tempName))
                    {
                        alTables.Add(tempName);
                    }

                }
            }

            if (alTables.Count == 0)
            {
                return null;
            }
            return alTables.ToArray();
        }

        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="excelVersion">excel版本</param>
        /// <param name="excelVersion">创建sheet的脚本</param>
        private static void CreateExcelFile(string filePath, string excelVersion, string createSql)
        {
            string outputDir = Path.GetDirectoryName(filePath);

            //导出路径不存在则创建
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            //导出文件不存在则创建，存在则重写

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    //创建2007Excel
                    fs.Write(Properties.Resources._2007, 0, Properties.Resources._2007.Length);
                }
                else
                {
                    //其他默认创建2003Excel
                    fs.Write(Properties.Resources._2003, 0, Properties.Resources._2003.Length);
                }

                //插入Sheet表。
                string connectionString = string.Format(m_excelVersion[excelVersion], filePath, "Yes");
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.CommandText = createSql;
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        /// <summary>
        /// 构建Excel列脚本。
        /// 格式如：Name VarChar，CreateDate Date
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        private static string CreateExcelColums(DataTable dtSource)
        {
            //检查列数
            if (dtSource.Columns.Count == 0)
            {
                throw new Exception("数据源列数为0");
            }
            //构建列
            StringBuilder sbColums = new StringBuilder();
            foreach (DataColumn dc in dtSource.Columns)
            {
                //注意表有可能使用系统关键字
                sbColums.AppendFormat(",[{0}] {1}", dc.ColumnName, GetExcelTypeByDataColumn(dc));
            }
            //去掉多余的逗号
            sbColums.Remove(0, 1);
            return sbColums.ToString();
        }

        /// <summary>
        /// 获取DataColumn对应的Excel列类型
        /// </summary>
        /// <param name="dc">源数据的列</param>
        /// <returns>Excel列类型名称</returns>
        private static string GetExcelTypeByDataColumn(DataColumn dc)
        {
            foreach (string key in m_excelTypeMap.Keys)
            {
                if (key == dc.DataType.Name)
                {
                    return m_excelTypeMap[dc.DataType.Name];
                }
            }
            return m_excelTypeMap[AppConfigKey.DefaultTypeKey];

        }

        /// <summary>
        /// 获取DataColumn对应的Excel列类型
        /// </summary>
        /// <param name="dc">源数据的列</param>
        /// <returns>Excel列类型名称</returns>
        private static OleDbType GetOleDbTypeByDataColumn(DataColumn dc)
        {
            switch (dc.DataType.Name)
            {
                case "String"://字符串
                    return OleDbType.VarChar;
                case "Double"://数字
                    return OleDbType.Double;
                case "Decimal"://数字
                    return OleDbType.Decimal;
                case "DateTime"://时间
                    return OleDbType.Date;
                default:
                    return OleDbType.VarChar;
            }
        }

        #endregion
    }
}
