// ================================================================================
// 		File: ExcelFile.cs
// 		Desc: Excel�����ࡣ
//            ��Ҫ����������Excel���ݵĵ��뵼����
//
// 		Called by:   
//               
// 		Auth: ����
// 		Date: 2010��8��20��
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
    /// Excel�ļ������ࡣ
    /// </summary>
    public class ExcelFile
    {
        #region ˽�б���

        //excel�汾��Ĭ��Ϊ98-2003��Excel��
        string m_version = string.Empty;

        //���Ӵ��е�HDR���ԣ����ڱ�ʶ��һ����������Ĭ���ǡ�
        string m_hdr = string.Empty;

        //excel�ļ�·��
        string m_filePath = string.Empty;

        //��ʱ��ŵ����ݱ�
        DataTable m_dataSource = null;

        //���澲̬����
        static NameValueCollection m_excelVersion = null;
        static NameValueCollection m_excelTypeMap = null;

        //ÿ��Sheel���ŵ��������
        static int m_maxSheelSize = 0;

        #endregion

        #region ���캯��
        /// <summary>
        /// ��̬���캯��
        /// </summary>
        static ExcelFile()
        {
            m_excelVersion = AppConfig.GetConfig(AppConfigKey.ExcelVersionKey);
            m_excelTypeMap = AppConfig.GetConfig(AppConfigKey.ExcelTypeKey);
            m_maxSheelSize = int.Parse(AppConfig.AppSettings[AppConfigKey.MaxSheelSize]);

            ////��m_excelVersionֵ�е�'�滻��"
            //foreach (string key in m_excelVersion.Keys)
            //{
            //    m_excelVersion[key] = m_excelVersion[key].Replace("'","\"");
            //}
        }
        /// <summary>
        /// �޲ι��캯��
        /// </summary>
        public ExcelFile() : this(null) { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="excelVersion">Excel�ĵ��汾��ʹ��ExcelVersion���͡�</param>
        public ExcelFile(string excelVersion)
        {
            //����Ĭ��ֵ
            m_version = ExcelVersion.Excel8;
            m_hdr = HDRType.Yes;


            //����Ĭ��ֵ
            if (!string.IsNullOrEmpty(excelVersion))
            {
                m_version = excelVersion;
            }
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��ȡ������Excel�ĵ��汾�š�
        /// ΪExcelVersion���͵�ֵ��
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
        /// ��ȡ������HDR���͡�
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
        /// ��ȡ������Excel�ļ�·����
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
        /// ��ȡ����������Դ��
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

        #region ���÷���
        /// <summary>
        /// ����DataSource��Excel��
        /// </summary>
        public void Import()
        {
            SetData(DataSource, FilePath, Version, HDR);
        }

        /// <summary>
        /// ��ȡExcel�е�����
        /// </summary>
        /// <param name="bMerge">��������Ƿ�ϲ�</param>
        /// <returns>DataTable����</returns>
        public DataTable[] GetData(bool bMerge)
        {
            return GetData(FilePath, Version, HDR, bMerge);
        }
        #endregion

        #region ���þ�̬����
        /// <summary>
        /// д���ݵ�Excel��
        /// </summary>
        /// <param name="dtSource">����Դ</param>
        /// <param name="filePath">Excel����·��</param>
        /// <param name="excelVersion">excel�汾��ΪExcelVersion����</param>
        /// <param name="pHDR">��һ���Ƿ���⣬ΪHDRType����</param>
        public static void SetData(DataTable dtSource, string filePath, string excelVersion, string pHDR)
        {
            //����ԴΪ��
            if (dtSource == null)
            {
                throw new Exception("�����ݿɵ�");
            }
            //����·��Ϊ��
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("δ����Excel����·��");
            }
            //ɾ���ļ�
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //�����ַ���
            string connectionString = string.Format(m_excelVersion[excelVersion], filePath, pHDR);

            // ����Excel 
            using (OleDbConnection Connection = new OleDbConnection(connectionString))
            {
                Connection.Open();

                //��������  
                using (OleDbCommand command = new OleDbCommand())
                {
                    command.Connection = Connection;

                    //������  ��ʽ�磺Name VarChar��CreateDate Date
                    string colList = CreateExcelColums(dtSource);

                    //��������SQL���
                    //��ʽ�� "INSERT INTO TABLE [tablename](col1,col2,col3) VALUES(@col1,@col2,@col3)"; 

                    StringBuilder sbColumNames = new StringBuilder();
                    StringBuilder sbColumValues = new StringBuilder();

                    foreach (DataColumn dc in dtSource.Columns)
                    {
                        sbColumNames.AppendFormat(",[{0}]", dc.ColumnName);
                        sbColumValues.AppendFormat(",@{0}", dc.ColumnName);
                    }

                    //ȥ������Ķ���
                    sbColumNames.Remove(0, 1);
                    sbColumValues.Remove(0, 1);

                    //������������ÿҳ���������ʱ���Զ���ҳ
                    int totalRows = dtSource.Rows.Count;//��������
                    int pageIndex = 0;

                    //��ʼ��������  do...whileѭ����Ϊ�˴����ҳ�߼�
                    do
                    {
                        //������ֲ����������
                        int insertRows = m_maxSheelSize - 1;

                        //�����������û�дﵽ����
                        if (totalRows < insertRows)
                        {
                            insertRows = totalRows;
                        }

                        string tableName = dtSource.TableName + pageIndex;
                        if (pageIndex == 0)
                        {
                            tableName = "Sheet1";
                        }

                        //��������
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
                        //    //���ʹ��Create��䴴��ʧ����ֱ�Ӵ���Excel�ļ�
                        //    CreateExcelFile(filePath, excelVersion, command.CommandText);
                        //}

                        //��������
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

                        //����ʣ��������
                        totalRows = totalRows - insertRows;
                        pageIndex++;

                    } while (totalRows > 0);
                }//end of using OleDbCommand  
            }// end of  using OleDbConnection
        }

        /// <summary>
        /// ��Excel������
        /// </summary>
        /// <param name="filePath">excel�ĵ�·��</param>
        /// <param name="excelVersion">�ĵ��汾</param>
        /// <param name="pHDR">��һ���Ƿ����</param>
        /// <param name="bMerge">
        /// ����ж�ҳ���Ƿ�ϲ����ݣ��ϲ�ʱ���뱣֤��ҳ�ı�ṹһ��
        /// </param>
        /// <returns>DataTable��</returns>
        public static DataTable[] GetData(string filePath, string excelVersion, string pHDR, bool bMerge)
        {
            List<DataTable> dtResult = new List<DataTable>();

            //m_excelVersion�������ֵ�Ǻ��ʱ������������أ� ʲôʹ��д��Ļ����أ�
            string connectionString = string.Format(m_excelVersion[excelVersion], filePath, pHDR);
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                string[] sheels = GetExcelWorkSheets(filePath, excelVersion);
                //GetExcelWorkSheets��������뱾�����Ĺ�ϵ������������õĹ�ϵ���и��ص����ע��
                //GetExcelWorkSheets���Զ�����ʹ��  ��϶Ȳ�̫ǿ
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

                        //��Ȼ��һ��excel�еĶ������ݱ���ȫ�Ķ������� ����д�����µ�excel�в�û�а�ԭ���Ľ���
                    }
                    catch
                    {
                        //�ݴ���ȡ����ʱ�������������Ϊ�ռ��ɡ�
                    }
                }

                //�����Ҫ�ϲ����ݣ���ϲ�����һ�ű�
                if (bMerge)
                {
                    for (int i = 1; i < dtResult.Count; i++)
                    {
                        //�����Ϊ�ղźϲ�
                        if (dtResult[0].Columns.Count == dtResult[i].Columns.Count &&
                            dtResult[i].Rows.Count > 0)
                        {
                            dtResult[0].Load(dtResult[i].CreateDataReader());
                        }
                    }
                }
            }
            //��ע��Ϣ�����ݲ�Ҫ׷�ӵ���һ�ű��ж���Ҫ�����������ݱ� ��ã������ܹ����ֺ�ԭ��һ���Ľṹ
            return dtResult.ToArray();
        }

        #endregion

        #region ˽�о�̬����

        /// <summary>
        /// ����ָ���ļ��������Ĺ������б�;�����WorkSheet���ͷ����Թ���������������ArrayList�����򷵻ؿ�
        /// </summary>
        /// <param name="filePath">Ҫ��ȡ��Excel</param>
        /// <param name="excelVersion">�ĵ��汾</param>
        /// <returns>�����WorkSheet���ͷ����Թ���������������string[]�����򷵻ؿ�</returns>
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
                    throw new Exception("�޷���ȡָ��Excel�ļܹ���");
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string tempName = dr["Table_Name"].ToString();

                    int iDolarIndex = tempName.IndexOf('$');

                    if (iDolarIndex > 0)
                    {
                        tempName = tempName.Substring(0, iDolarIndex);
                    }

                    //����Excel2003��ĳЩ����������Ϊ���ֵı��޷���ȷʶ���BUG��
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
        /// ����Excel�ļ�
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="excelVersion">excel�汾</param>
        /// <param name="excelVersion">����sheet�Ľű�</param>
        private static void CreateExcelFile(string filePath, string excelVersion, string createSql)
        {
            string outputDir = Path.GetDirectoryName(filePath);

            //����·���������򴴽�
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            //�����ļ��������򴴽�����������д

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    //����2007Excel
                    fs.Write(Properties.Resources._2007, 0, Properties.Resources._2007.Length);
                }
                else
                {
                    //����Ĭ�ϴ���2003Excel
                    fs.Write(Properties.Resources._2003, 0, Properties.Resources._2003.Length);
                }

                //����Sheet��
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
        /// ����Excel�нű���
        /// ��ʽ�磺Name VarChar��CreateDate Date
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        private static string CreateExcelColums(DataTable dtSource)
        {
            //�������
            if (dtSource.Columns.Count == 0)
            {
                throw new Exception("����Դ����Ϊ0");
            }
            //������
            StringBuilder sbColums = new StringBuilder();
            foreach (DataColumn dc in dtSource.Columns)
            {
                //ע����п���ʹ��ϵͳ�ؼ���
                sbColums.AppendFormat(",[{0}] {1}", dc.ColumnName, GetExcelTypeByDataColumn(dc));
            }
            //ȥ������Ķ���
            sbColums.Remove(0, 1);
            return sbColums.ToString();
        }

        /// <summary>
        /// ��ȡDataColumn��Ӧ��Excel������
        /// </summary>
        /// <param name="dc">Դ���ݵ���</param>
        /// <returns>Excel����������</returns>
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
        /// ��ȡDataColumn��Ӧ��Excel������
        /// </summary>
        /// <param name="dc">Դ���ݵ���</param>
        /// <returns>Excel����������</returns>
        private static OleDbType GetOleDbTypeByDataColumn(DataColumn dc)
        {
            switch (dc.DataType.Name)
            {
                case "String"://�ַ���
                    return OleDbType.VarChar;
                case "Double"://����
                    return OleDbType.Double;
                case "Decimal"://����
                    return OleDbType.Decimal;
                case "DateTime"://ʱ��
                    return OleDbType.Date;
                default:
                    return OleDbType.VarChar;
            }
        }

        #endregion
    }
}
