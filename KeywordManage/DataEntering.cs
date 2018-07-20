using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using KM.Common;

namespace KeywordManage
{
    public partial class DataEntering : Form
    {
        //操作类型
        public string actionType = string.Empty;

        //更新进度列表 
        private delegate void SetPos(int ipos);

        public DataEntering()
        {
            InitializeComponent();
        }

        public DataEntering(string type)
        {
            InitializeComponent();
            this.actionType = type;
        }

        //数据录入  关键字录入  站点信息录入 
        private void DataEntering_Load(object sender, EventArgs e)
        {

        }
        //设置
        private void btnSet_Click(object sender, EventArgs e)
        {
            //需要支持Excel文件
            string strTxtpath = Tools.GetFilePath(new OpenFileDialog(), "Excel2003文件(*.xls)|*.xls|Excel2007文件(*.xlsx)|*.xlsx|txt文件(*.txt)|*.txt");
            if (strTxtpath == "")
            {
                return;
            }
            this.comboxkey.Text = strTxtpath;
        }

        //批量导入
        private void btnImport_Click(object sender, EventArgs e)
        {
            Thread fThread = new Thread(new ThreadStart(SleepT));//开辟一个新的线程 
            fThread.Start();
        }

        //窗体初始化

        //更新界面
        private void SetTextMessage(int ipos)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetTextMessage);
                this.Invoke(setpos, new object[] { ipos });
            }
            else
            {
              this.label1.Text = ipos.ToString() + "/100";
                this.progressBar1.Value = Convert.ToInt32(ipos);
            }
        }

        private void SleepT()
        {
            for (int i = 1; i < 500; i++)
            {
                System.Threading.Thread.Sleep(10);//没什么意思，单纯的执行延时 
                SetTextMessage(100 * i / 500);
            }
            //new KM.DataOpear.KeyWords().ImporData(this.comboxkey.Text);
        }
    }
}
