using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace KM.Common
{
    public class Tools
    {
        #region 设置页面按钮的提示情况
        /// <summary>
        /// 设置页面按钮的提示情况
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="message"></param>
        public static void SetTip(Control cl, string message)
        {
            ToolTip tp = new ToolTip();
            tp.ShowAlways = true;
            tp.SetToolTip(cl, message);
        }
        #endregion

        #region 文件对话框设置函数部分

        /// <summary>
        /// 文件对话框设置函数部分
        /// </summary>
        /// <param name="ofd">对话框选项</param>
        /// <param name="strFilter">文件过滤表达式</param>
        /// <returns>文件的路径</returns>
        public static string GetFilePath(OpenFileDialog ofd, string strFilter)
        {
            OpenFileDialog ofdExcel = new OpenFileDialog();
            ofdExcel.Filter = strFilter;
            ofdExcel.AddExtension = true;
            //ofdExcel.InitialDirectory = strTxtpath;
            string strexcelPath = string.Empty;
            if (ofdExcel.ShowDialog() == DialogResult.OK)
            {
                strexcelPath = ofdExcel.FileName;
            }
            return strexcelPath;
        }
        #endregion

        #region 加载文本关键词数据部分
        /// <summary>
        /// 加载文本关键词数据部分到list集合
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> GetkeyWords(string filePath)
        {
            List<string> list = new List<string>();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            list.Add(line.Trim());//去空处理
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            fs.Close();
            return list;
        } 
        #endregion
    }
}
