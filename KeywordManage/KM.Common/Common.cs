#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Data.SqlClient;
using System.Security.Cryptography;
#endregion

/*通用函数助手*/
namespace CommonSpace
{
    public enum ExcelVersions
    {
        //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=;Extended properties=Excel 12.0;Imex=1;HDR=Yes;
        //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=;Extended Properties=Excel 12.0 Xml;HDR=YES;IMEX=1;

        ///// <summary>
        ///// Excel3.0版文档格式
        ///// </summary>
        //Excel3 = "Excel3.0",
        ///// <summary>
        ///// Excel4.0版文档格式
        ///// </summary>
        //Excel4 = "Excel4.0",
        ///// <summary>
        ///// Excel5.0版文档格式，适用于 Microsoft Excel 5.0 和 7.0 (95) 工作簿
        ///// </summary>
        //Excel5 = "Excel5.0",

        /// <summary>
        /// Excel8.0版文档格式，适用于Microsoft Excel 8.0 (98-2003) 工作簿
        /// </summary>
        Excel8 = 2003,
        /// <summary>
        /// Excel12.0版文档格式，适用于Microsoft Excel 12.0 (2007) 工作簿
        /// </summary>
        Excel12 = 2007
    }

    public class Conmmon
    {
        #region 功能：用于读取显示txt文件================================

        /// <summary>
        /// 用于读取txt文件显示输出
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        public static void LoadDisText(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            fs.Close();
        }

        public static string[] LoadText(string filePath)
        {
            ArrayList alist = new ArrayList();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        alist.Add(line);
                    }
                }
                string[] arr = new string[alist.Count];
                for (int i = 0; i < alist.Count; i++)
                {
                    arr[i] = alist[i].ToString();
                }
                return arr;
            }
        }

        /// <summary>
        /// 读取一个文件并返回全部的内容
        /// </summary>
        /// <param name="filePath">文本文件的地址</param>
        /// <returns>文本文件的全部内容</returns>
        public static string ReadTextToend(string filePath)
        {
            String strContent = string.Empty;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                        strContent = sr.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    fs.Dispose();
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            return strContent;
        }

        #endregion

        #region 功能：遍历一个DataTable==================================

        /// <summary>
        /// 遍历一个DataTable
        /// </summary>
        /// <param name="dt"></param>
        public static void ErgodicDataTable(DataTable dt)
        {
            #region 001
            //if (dt != null)
            //{
            //    int col = dt.Columns.Count;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        for (int i = 0; i < col; i++)
            //        {
            //            Console.Write("{0}\t", dr[i].ToString());
            //        }
            //        Console.WriteLine();
            //    }
            //}
            #endregion

            #region 002
            //int row = dt.Rows.Count;
            //int col = dt.Columns.Count;
            //for (int j = 0; j < row; j++)
            //{
            //    for (int k = 0; k < col; k++)
            //    {
            //        Console.Write("{0}\t",dt.Rows[j][k].ToString());
            //    }
            //    Console.WriteLine();
            //} 
            #endregion

            #region 003
            DataRow[] dr = dt.Select();
            int col = dt.Columns.Count;
            for (int k = 0; k < dr.Length; k++)
            {
                for (int l = 0; l < col; l++)
                {
                    Console.Write("{0}\t", dr[k][l].ToString());
                }
                Console.WriteLine();
            } 
            #endregion

            #region 004
            //foreach (DataRow dr in dt.Rows)
            //{
            //    foreach (DataColumn dc in dt.Columns)
            //    {
            //        Console.WriteLine(dr[dt].ToString());
            //    }
            //} 
            #endregion
        }
        #endregion

        #region 功能：遍历一个ArrayList==================================

        /// <summary>
        /// 遍历一个ArrayList
        /// </summary>
        /// <param name="alist"></param>
        public static void ErgodicAlist(ArrayList alist)
        {
            if (alist != null)
            {
                for (int i = 0; i < alist.Count; i++)
                {
                    Console.WriteLine(alist[i].ToString());
                }
            }
        }

        #endregion

        #region 功能：读取excel文件获得DataTable=========================

        /// <summary>
        /// 读取excel文件获得DataTable
        /// </summary>
        /// <param name="strExcelFileName">目标Excel文件完全路径</param>
        /// <param name="strSheetName">工作表的名字</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strExcelFileName, string strSheetName, ExcelVersions exVersions)
        {
            string ConnectString = string.Empty;
            switch (exVersions)
            {
                case ExcelVersions.Excel8:
                    ConnectString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
                    break;
                case ExcelVersions.Excel12:
                    ConnectString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';";
                    break;
                default:
                    break;
            }
            string strExcel = "select * from  [" + strSheetName + "$]";
            DataSet ds = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(ConnectString))
            {
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, ConnectString);
                adapter.Fill(ds, strSheetName);
                return ds.Tables[strSheetName];
            }
            #region MyRegion

            //很简单的代码，但是问题就出在连接字符串上面，后面一定要加上Extended Properties='Excel 8.0;HDR=NO;IMEX=1'，HDR和IMEX也一定要配合使用，
            //哈哈,老实说,我也不知道为什么,这样配合的效果最好,这是我艰苦调试的结果.IMEX=1应该是将所有的列全部视为文本,我也有点忘记了.
            //至于HDR本来只是说是否要出现一行标题头而已,但是结果却会导致某些字段值丢失,所以其实我至今也搞不明白为什么,很可能是驱动的问题...
            //IMEX=1 解决数字与字符混合时,识别不正常的情况.
            #endregion
        }
        /// <summary>
        /// 读取excel文件获得DataReader
        /// </summary>
        /// <param name="strExcelFileName"></param>
        /// <param name="strSheetName"></param>
        /// <returns></returns>
        public static OleDbDataReader ExcelToDataReader(string strExcelFileName, string strSheetName)
        {
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';";
            string strExcel = "select * from  [" + strSheetName + "$]";

            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbCommand cmd = new OleDbCommand(strExcel, conn);
            try
            {
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region 功能：创建指定格式的文件=================================

        /// <summary>
        /// 创建指定格式的文件
        /// </summary>
        /// <param name="strContent">写入的文件内容</param>
        /// <param name="strGobalPaht">保存的路径</param>
        /// <returns></returns>
        public static bool CreateFile(string strContent, string strGobalPaht)
        {
            try
            {
                using (FileStream fs = new FileStream(strGobalPaht, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fs, Encoding.Default))
                    {
                        sr.WriteLine(strContent);
                    }
                }
                return true;
            }
            catch
            {
                //throw new Exception(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 创建指定格式的文件
        /// </summary>
        /// <param name="strContent">写入的文件内容</param>
        /// <param name="strDir">保存的目录</param>
        /// <param name="strName">保存的文件名</param>
        /// <param name="strType">保存的文件类型</param>
        /// <returns></returns>
        public static bool CreateFile(string strContent, string strDir, string strName, string strType)
        {
            try
            {
                if (!Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }
                using (FileStream fs = new FileStream(strDir + strName + strType, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fs, Encoding.Default))
                    {
                        sr.WriteLine(strContent);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建指定格式的文件
        /// </summary>
        /// <param name="strPath1"></param>
        /// <returns></returns>
        public static bool CreateFile(string strPath)
        {
            bool bl = true;
            try
            {
                //string strPath = "E:\\m.txt";
                if (File.Exists(strPath))
                {
                    File.Delete(strPath);
                    File.Create(strPath);
                    bl = true;
                }
                else
                {
                    File.Create(strPath);
                    bl = true;
                }
            }
            catch
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region 功能：递归算法将字符串分割成字符串数组===================

        /* 
            "asdfasfasdfasdfasdf"
            "sfasdfasdfasdf"
            "sdfasdfasdf"
            "sdfasdf"
            "sdf"
            "sdfasdf"
            "sdfasdfasdf"
            "sfasdfasdfasdf"
            "asdfasfasdfasdfasdf"
            */

        /// <summary>
        /// 将字符串分割成数组
        /// </summary>
        /// <param name="strSource">目标字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <returns>分割后的字符数组</returns>
        /// 返回的内容中并不包含所谓的分隔符号
        public static string[] StringSplit(string strSource, string strSplit)
        {
            string[] strtmp = new string[1];

            int index = strSource.IndexOf(strSplit, 0);//得到分割符出现的第一个位置

            if (index < 0)//表示没有找到该分隔符
            {
                strtmp[0] = strSource;
                return strtmp;//返回目标的字符
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);//没什么价值

                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }

        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource">目标源</param>
        /// <param name="strSplit">分割符号</param>
        /// <param name="attachArray">附加数组</param>
        /// <returns></returns>
        public static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
        {
            string[] strtmp = new string[attachArray.Length + 1];//临时

            attachArray.CopyTo(strtmp, 0);

            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }

        #endregion

        #region 功能：获取指定目录下的指定类型的文件=====================

        /// <summary>
        /// 获取指定目录下的指定类型的文件
        /// </summary>
        /// <param name="strDir">目录路径</param>
        /// <param name="strFileType">文件类型【*.txt】</param>
        /// <param name="bl">是否返回完全的路径</param>
        /// <returns></returns>
        public static string[] GetDirFile(string strDir, string strFileType, bool bl)
        {
            if (!string.IsNullOrEmpty(strDir) && strDir != null)
            {
                DirectoryInfo dir = new DirectoryInfo(strDir);
                int lenth = dir.GetFiles(strFileType).Length;
                string[] arr = new string[lenth];
                int i = 0;

                foreach (FileInfo dChild in dir.GetFiles(strFileType))
                {
                    if (bl)
                    {
                        arr[i] = dChild.FullName;
                    }
                    else
                    {
                        arr[i] = dChild.Name;
                    }
                    i++;
                }
                return arr;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 功能：创建目录===========================================

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath"></param>
        public static bool CreateDir(string directoryPath)
        {
            bool bl = true;
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath);
                    Directory.CreateDirectory(directoryPath);
                    bl = true;
                }
                else
                {
                    Directory.CreateDirectory(directoryPath);
                    bl = true;
                }
            }
            catch (Exception)
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region 功能：替换字符串=========================================

        /// <summary>
        /// 功能：替换字符串
        /// </summary>
        /// <param name="strSource">目标字符串</param>
        /// <param name="oldStr">要替换的字符串</param>
        /// <param name="strNew">替换成</param>
        /// <returns></returns>
        public static string ReplaceImgURL(string strSource, string oldStr, string strNew)
        {
            if (!string.IsNullOrEmpty(strSource) && !string.IsNullOrEmpty(oldStr) && !string.IsNullOrEmpty(strNew))
            {
                return strSource.Replace(oldStr, strNew);
            }
            else
            {
                return strSource;
            }

        }

        #endregion

        #region 功能：取得HTML中所有图片的 URL===========================

        /// <summary> 
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }
        /// <summary> 
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string GetHtmlImageUrlList2(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);

            StringBuilder sbr = new StringBuilder();

            // 取得匹配项列表 
            foreach (Match match in matches)
            {
                sbr.Append(match.Groups["imgUrl"].Value + Environment.NewLine);
            }
            return sbr.ToString();
        }

        #endregion

        #region 返回两个日期之间的时间间隔===============================
        ///   <summary>   
        ///   返回两个日期之间的时间间隔（y：年份间隔、M：月份间隔、d：天数间隔、h：小时间隔、m：分钟间隔、s：秒钟间隔、ms：微秒间隔）   
        ///   </summary>   
        ///   <param   name="Date1">开始日期</param>   
        ///   <param   name="Date2">结束日期</param>   
        ///   <param   name="Interval">间隔标志</param>   
        ///   <returns>返回间隔标志指定的时间间隔</returns>   
        public static int DateDiff(System.DateTime Date1, System.DateTime Date2, string Interval)
        {
            double dblYearLen = 365;//年的长度，365天   
            double dblMonthLen = (365 / 12);//每个月平均的天数   
            System.TimeSpan objT;
            objT = Date2.Subtract(Date1);
            switch (Interval)
            {
                case "y"://返回日期的年份间隔   
                    return System.Convert.ToInt32(objT.Days / dblYearLen);
                case "M"://返回日期的月份间隔   
                    return System.Convert.ToInt32(objT.Days / dblMonthLen);
                case "d"://返回日期的天数间隔   
                    return objT.Days;
                case "h"://返回日期的小时间隔   
                    return objT.Hours;
                case "m"://返回日期的分钟间隔   
                    return objT.Minutes;
                case "s"://返回日期的秒钟间隔   
                    return objT.Seconds;
                case "ms"://返回时间的微秒间隔   
                    return objT.Milliseconds;
                default:
                    break;
            }
            return 0;
        }
        #endregion

        #region 获得一个不重复的字符串===================================
        /// <summary>
        /// 获得一个不重复的字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 获得一个不重复的长整型数字
        /// </summary>
        /// <returns></returns>
        public static long GenerateIntID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
        #endregion

        #region 日志函数=================================================
        /// <summary>
        /// 日志函数
        /// </summary>
        /// <param name="logpath">保存日志的地址</param>
        /// <returns></returns>
        public static bool SetLog(string logpath, string strcontent)
        {
            bool bl = true;
            try
            {
                using (System.IO.FileStream fs = new FileStream(logpath, FileMode.Append, FileAccess.Write))
                {
                    using (System.IO.StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(strcontent);
                    }
                }
            }
            catch
            {
                bl = false;
            }
            return bl;
        }
        #endregion
    }

    public class CNHelper
    {
        #region 获取汉字拼音的第一个字母===================
        /// <summary>
        /// 获取汉字拼音的第一个字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText, bool IsToUpper)
        {
            int len = strText.Length;
            string myStr = string.Empty;

            for (int i = 0; i < len; i++)
            {
                if (CheckEnChar(strText.Substring(i, 1)))//判断这个字符是否是汉字  
                {
                    myStr += GetSpell(strText.Substring(i, 1));
                }
            }
            if (IsToUpper)
            {
                return myStr;
            }
            else
            {
                return myStr.ToLower();
            }
        }
        #endregion

        #region 检测中文字符===============================
        /// <summary>
        /// 检测中文字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool CheckEnChar(string text)
        {
            return Regex.IsMatch(text, "[\u4e00-\u9fa5]");
        }
        #endregion

        #region 获得中文字符的首字母=======================
        /// <summary>
        /// 获得中文字符的首字母
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        public static string GetSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);

            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;

                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };

                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25)
                    {
                        max = areacode[i + 1];
                    }
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else
            {
                return cnChar;
            }
        }

        #endregion

        #region 中文转16进制===============================
        /// <summary>
        /// 中文转16进制
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        public static string ConvetTo16(string cnChar)
        {
            //string str = "中文";
            string outStr = "";
            if (!string.IsNullOrEmpty(cnChar))
            {
                for (int i = 0; i < cnChar.Length; i++)
                {
                    //將中文轉為10進制整數，然後轉為16進制unicode 
                    outStr += "\\u" + ((int)cnChar[i]).ToString("x");
                }
            }
            //Console.WriteLine(outStr);
            //需要检测是否是中文字符
            return outStr;
        }

        #endregion

        #region 16进制转中文===============================

        /// <summary>
        /// 16进制转中文
        /// </summary>
        /// <param name="strUnicode"></param>
        /// <returns></returns>
        public static string ConvertToCN(string strUnicode)
        {
            //string str = "\\u4e2d\\u6587";
            string outStr = "";

            if (!string.IsNullOrEmpty(strUnicode))
            {
                string[] strlist = strUnicode.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //將unicode轉為10進制整數，然後轉為char中文
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            // Console.WriteLine(outStr);
            return outStr;
        }

        #endregion
    }

    public class ToolHelper
    {
        #region 功能：MD5加密函数

        /// <summary>
        /// MD5加密函数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DataToMd5(string data)
        {
            string cl = data;
            string pwd = string.Empty;

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                #region note
                // 将得到的字符串使用十六进制类型格式。
                // 格式后的字符是小写的字母，
                // 如果使用大写（X）则格式后的字符是大写字符
                // 将生成的字符转换成大写
                // pwd = pwd + s[i].ToString("X"); 
                #endregion
                pwd += s[i].ToString("X");
                Console.WriteLine(s[i].ToString());
            }
            return pwd;
        }
        #endregion
    }

    /// <summary>
    /// 汉字转拼音工具
    /// </summary>
    public sealed class CHS2PinYin
    {
        #region 字段属性======================
        /// <summary>
        /// 包含字符 ASC 码的整形数组
        /// </summary>
        private static int[] pv = new int[] { -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036, -20032, -20026, -20002, -19990, -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, -19741, -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515, -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, -19263, -19261, -19249, -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018, -19006, -19003, -18996, -18977, -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490, -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012, -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752, -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983, -16970, -16942, -16915, -16733, -16708, -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448, -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216, -16212, -16205, -16202, -16187, -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701, -15681, -15667, -15661, -15659, -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394, -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, -14921, -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668, -14663, -14654, -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345, -14170, -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112, -14109, -14099, -14097, -14094, -14092, -14090, -14087, -14083, -13917, -13914, -13910, -13907, -13906, -13905, -13896, -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601, -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356, -13343, -13340, -13329, -13326, -13318, -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060, -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831, -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300, -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067, -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018, -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328, -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254 };

        /// <summary>
        /// 包含汉字拼音的字符串数组
        /// </summary>
        private static string[] ps = new string[] { "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao", "bei", "ben", "beng", "bi", "bian", "biao", "bie", "bin", "bing", "bo", "bu", "ca", "cai", "can", "cang", "cao", "ce", "ceng", "cha", "chai", "chan", "chang", "chao", "che", "chen", "cheng", "chi", "chong", "chou", "chu", "chuai", "chuan", "chuang", "chui", "chun", "chuo", "ci", "cong", "cou", "cu", "cuan", "cui", "cun", "cuo", "da", "dai", "dan", "dang", "dao", "de", "deng", "di", "dian", "diao", "die", "ding", "diu", "dong", "dou", "du", "duan", "dui", "dun", "duo", "e", "en", "er", "fa", "fan", "fang", "fei", "fen", "feng", "fo", "fou", "fu", "ga", "gai", "gan", "gang", "gao", "ge", "gei", "gen", "geng", "gong", "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun", "guo", "ha", "hai", "han", "hang", "hao", "he", "hei", "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan", "huang", "hui", "hun", "huo", "ji", "jia", "jian", "jiang", "jiao", "jie", "jin", "jing", "jiong", "jiu", "ju", "juan", "jue", "jun", "ka", "kai", "kan", "kang", "kao", "ke", "ken", "keng", "kong", "kou", "ku", "kua", "kuai", "kuan", "kuang", "kui", "kun", "kuo", "la", "lai", "lan", "lang", "lao", "le", "lei", "leng", "li", "lia", "lian", "liang", "liao", "lie", "lin", "ling", "liu", "long", "lou", "lu", "lv", "luan", "lue", "lun", "luo", "ma", "mai", "man", "mang", "mao", "me", "mei", "men", "meng", "mi", "mian", "miao", "mie", "min", "ming", "miu", "mo", "mou", "mu", "na", "nai", "nan", "nang", "nao", "ne", "nei", "nen", "neng", "ni", "nian", "niang", "niao", "nie", "nin", "ning", "niu", "nong", "nu", "nv", "nuan", "nue", "nuo", "o", "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen", "peng", "pi", "pian", "piao", "pie", "pin", "ping", "po", "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin", "qing", "qiong", "qiu", "qu", "quan", "que", "qun", "ran", "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou", "ru", "ruan", "rui", "run", "ruo", "sa", "sai", "san", "sang", "sao", "se", "sen", "seng", "sha", "shai", "shan", "shang", "shao", "she", "shen", "sheng", "shi", "shou", "shu", "shua", "shuai", "shuan", "shuang", "shui", "shun", "shuo", "si", "song", "sou", "su", "suan", "sui", "sun", "suo", "ta", "tai", "tan", "tang", "tao", "te", "teng", "ti", "tian", "tiao", "tie", "ting", "tong", "tou", "tu", "tuan", "tui", "tun", "tuo", "wa", "wai", "wan", "wang", "wei", "wen", "weng", "wo", "wu", "xi", "xia", "xian", "xiang", "xiao", "xie", "xin", "xing", "xiong", "xiu", "xu", "xuan", "xue", "xun", "ya", "yan", "yang", "yao", "ye", "yi", "yin", "ying", "yo", "yong", "you", "yu", "yuan", "yue", "yun", "za", "zai", "zan", "zang", "zao", "ze", "zei", "zen", "zeng", "zha", "zhai", "zhan", "zhang", "zhao", "zhe", "zhen", "zheng", "zhi", "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan", "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", "zou", "zu", "zuan", "zui", "zun", "zuo" };

        /// <summary>
        /// 包含要排除处理的字符的字符串数组
        /// </summary>
        private static string[] bd = new string[] { "，", "", "“", "”", "‘", "’", "￥", "$", "（", "「", "『", "）", "」", "』", "［", "〖", "【", "］", "〗", "】", "—", "…", "《", "＜", "》", "＞" };

        private static Hashtable _Phrase;

        /// <summary>
        /// 设置或获取包含列外词组读音的键/值对的组合
        /// </summary>
        public static Hashtable Phrase
        {
            get
            {
                if (_Phrase == null)
                {
                    _Phrase = new Hashtable();

                    _Phrase.Add("重庆", "Chong Qing");
                    _Phrase.Add("深圳", "Shen Zhen");
                    _Phrase.Add("什么", "Shen Me");
                }

                return _Phrase;
            }

            set
            {
                _Phrase = value;
            }
        }
        #endregion

        #region 方法==========================

        /// <summary>
        /// 将指定中文字符串转换为拼音形式
        /// </summary>
        /// <param name="chs">要转换的中文字符串</param>
        /// <param name="separator">连接拼音之间的分隔符</param>
        /// <param name="initialCap">指定是否将首字母大写</param>
        /// <returns>包含中文字符串的拼音的字符串</returns>
        public static string Convert(string chs, string separator, bool initialCap)
        {
            if (chs == null || chs.Length == 0)
            {
                return "";
            }
            if (separator == null || separator.Length == 0)
            {
                separator = "";
            }

            // 例外词组
            foreach (DictionaryEntry de in CHS2PinYin.Phrase)
            {
                chs = chs.Replace(de.Key.ToString(), String.Format(" {0} ", de.Value.ToString().Replace(" ", separator)));
            }

            byte[] array = new byte[2];

            string returnstr = "";
            int chrasc = 0;
            int i1 = 0;
            int i2 = 0;
            bool b = false;
            char[] nowchar = chs.ToCharArray();

            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            TextInfo ti = ci.TextInfo;

            for (int j = 0; j < nowchar.Length; j++)
            {
                array = Encoding.Default.GetBytes(nowchar[j].ToString());
                string s = nowchar[j].ToString();
                ;

                if (array.Length == 1)
                {
                    b = true;
                    returnstr += s;
                }
                else
                {
                    if (s == "？")
                    {
                        if (returnstr == "" || b == true)
                            returnstr += s;
                        else
                            returnstr += separator + s;

                        continue;
                    }

                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);

                    chrasc = i1 * 256 + i2 - 65536;

                    for (int i = (pv.Length - 1); i >= 0; i--)
                    {
                        if (pv[i] <= chrasc)
                        {
                            s = ps[i];

                            if (initialCap == true)
                            {
                                s = ti.ToTitleCase(s);
                            }

                            if (returnstr == "" || b == true)
                            {
                                returnstr += s;
                            }
                            else
                            {
                                returnstr += separator + s;
                            }
                            break;
                        }
                    }

                    b = false;
                }
            }

            returnstr = returnstr.Replace(" ", separator);
            return returnstr;
        }

        /// <summary>
        /// 将指定中文字符串转换为拼音形式
        /// </summary>
        /// <param name="chs">要转换的中文字符串</param>
        /// <param name="separator">连接拼音之间的分隔符</param>
        /// <returns>包含中文字符串的拼音的字符串</returns>
        public static string Convert(string chs, string separator)
        {
            return CHS2PinYin.Convert(chs, separator, false);
        }

        /// <summary>
        /// 将指定中文字符串转换为拼音形式
        /// </summary>
        /// <param name="chs">要转换的中文字符串</param>
        /// <param name="initialCap">指定是否将首字母大写</param>
        /// <returns>包含中文字符串的拼音的字符串</returns>
        public static string Convert(string chs, bool initialCap)
        {
            return CHS2PinYin.Convert(chs, "", initialCap);
        }

        /// <summary>
        /// 将指定中文字符串转换为拼音形式
        /// </summary>
        /// <param name="chs">要转换的中文字符串</param>
        /// <returns>包含中文字符串的拼音的字符串</returns>
        public static string Convert(string chs)
        {
            return CHS2PinYin.Convert(chs, "");
        }
        #endregion
    }

    /// <summary>
    /// 指定长度的随机字符串
    /// </summary>
    public static class RandomCode
    {
        //public static int rep = 0;
        public static int rep = new Random().Next(0, 10000);
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;

            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
    }

    public static class RandomString
    {
        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="charList"></param>
        /// <returns></returns>
        private static List<string> SortByRandom(List<string> charList)
        {
            Random rand = new Random();
            for (int i = 0; i < charList.Count; i++)
            {
                int index = rand.Next(0, charList.Count);
                string temp = charList[i];
                charList[i] = charList[index];
                charList[index] = temp;
            }

            return charList;
        }

        private static void ShowError(string strError)
        {
            //MessageBox.Show(strError, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<string> GetRandString(int len, int count)
        {
            double max_value = Math.Pow(36, len);
            if (max_value > long.MaxValue)
            {
                //ShowError(string.Format("Math.Pow(36, {0}) 超出 long最大值！", len));
                return null;
            }

            long all_count = (long)max_value;
            long stepLong = all_count / count;
            if (stepLong > int.MaxValue)
            {
                //ShowError(string.Format("stepLong ({0}) 超出 int最大值！", stepLong));
                return null;
            }
            int step = (int)stepLong;
            if (step < 3)
            {
                //ShowError("step 不能小于 3!");
                return null;
            }
            long begin = 0;
            List<string> list = new List<string>();
            Random rand = new Random();
            while (true)
            {
                long value = rand.Next(1, step) + begin;
                begin += step;
                list.Add(GetChart(len, value));
                if (list.Count == count)
                {
                    break;
                }
            }

            list = SortByRandom(list);

            return list;
        }

        //数字+字母
        private const string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 将数字转化成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetChart(int len, long value)
        {
            StringBuilder str = new StringBuilder();
            while (true)
            {
                str.Append(CHAR[(int)(value % 36)]);
                value = value / 36;
                if (str.Length == len)
                {
                    break;
                }
            }

            return str.ToString();
        }
    }
}
