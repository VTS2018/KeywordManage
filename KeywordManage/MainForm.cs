using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeywordManage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //关键词管理
        private void toolStripKeyMenu_Click(object sender, EventArgs e)
        {
            ShowChildForm<KeywordManage>();
        }

        //退出系统
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("你确定要退出？", "关闭系统", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = false;
                    Application.ExitThread();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        //公共代码
        #region 单键模式，确保子窗体只能打开一次
        public void ShowChildForm<T>()
        {
            //通过数据类型创建泛型对象
            Form newForm = (Form)Activator.CreateInstance(typeof(T));

            bool bl = true;//默认该窗体不存在
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == newForm.GetType())
                {
                    frm.Activate();
                    bl = false;
                }

            }
            if (bl)
            {
                newForm.MdiParent = this;
                newForm.Show();
            }
        }
        #endregion
    }
}