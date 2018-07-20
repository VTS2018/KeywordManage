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
    public partial class KeywordEdit : Form
    {
        public string kid = "";
        public string action = string.Empty;
        public KM.DataOpear.KeyWords kw = new KM.DataOpear.KeyWords();

        public KeywordEdit()
        {
            InitializeComponent();
        }

        public KeywordEdit(string strkid, string straction)
        {
            InitializeComponent();
            this.kid = strkid;
            this.action = straction;
        }

        //更新
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (this.action == "编辑")
            {
                //编辑操作
                if (!Doedit())
                {
                    MessageBox.Show("更新失败了！");
                }
                MessageBox.Show("更新成功了！");
            }
            else if (this.action == "添加")
            {
                //添加操纵
                if (!DoAdd())
                {
                    MessageBox.Show("更新失败了！");
                }
                MessageBox.Show("更新成功了！");
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //编辑
        private void KeywordEdit_Load(object sender, EventArgs e)
        {
            LoadCombox();
            BingData();
        }

        //赋值操作
        public void BingData()
        {
            if (this.kid != "")
            {
                KM.Entity.KeyWords kwmodel = kw.GetModel(this.kid);

                this.txtID.Text = kwmodel.KID;
                this.txtName.Text = kwmodel.KeyWordsName;
                this.comBoxStatus.SelectedItem = kwmodel.KeyWordsStatus;

                //禁用只读
                this.txtID.ReadOnly = true;
            }
        }

        //增加操作
        public bool DoAdd()
        {
            bool bl = true;

            KM.Entity.KeyWords kwmodel = new KM.Entity.KeyWords();
            kwmodel.KID = CommonSpace.Conmmon.GenerateStringID();
            kwmodel.KeyWordsName = this.txtName.Text;
            kwmodel.KeyWordsStatus = this.comBoxStatus.Text;

            if (!kw.Add(kwmodel))
            {
                bl = false;
            }
            return bl;
        }

        //编辑操作
        public bool Doedit()
        {
            //为什么在这个地方还要获取一次数据呢？
            //因为需要的将获得更新对象的ID
            bool bl = true;
            if (this.kid != "")
            {
                KM.Entity.KeyWords kwmodel = kw.GetModel(this.kid);
                kwmodel.KeyWordsName = this.txtName.Text;
                kwmodel.KeyWordsStatus = this.comBoxStatus.Text;
                if (!kw.Update(kwmodel))
                {
                    bl = false;
                }
            }
            return bl;
        }

        //赋值操作com
        public void LoadCombox()
        {
            this.comBoxStatus.Items.Clear();
            this.comBoxStatus.Items.AddRange(new string[] { "yes", "no" });
            this.comBoxStatus.SelectedItem = this.comBoxStatus.Items[0];
        }
    }
}
