using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KM.Common;
using KM.DataOpear;

namespace KeywordManage
{
    public partial class KeywordManage : Form
    {
        KeyWords keys = new KeyWords();

        /// <summary>
        /// 搜索条件
        /// </summary>
        protected string strWhere = "";

        /// <summary>
        /// 默认排序字段
        /// </summary>
        protected string strfiledOrder = "关键字ID asc";

        /// <summary>
        /// 影片总数
        /// </summary>
        protected int totalCount = 0;

        /// <summary>
        /// 当前分页索引
        /// </summary>
        protected int pageCurrent = 0;

        /// <summary>
        /// 总页数＝总记录数/每页显示行数  
        /// </summary>
        protected int pageCount = 0;

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        protected int pageSize = 0;

        /// <summary>
        /// 关键字
        /// </summary>
        protected string keyWord = string.Empty;

        protected DataTable dtinfo = new DataTable();

        public KeywordManage()
        {
            InitializeComponent();
        }

        //初始加载
        private void KeywordManage_Load(object sender, EventArgs e)
        {
            InitVar();
            InitDataGridView(this.dataGridKeyWord);
            CountPage();
            this.btnChoose.Text = "全 选";
            //使用另一个线程来加载数据
            BingMovie(this.pageSize, this.pageCurrent, this.strWhere, this.strfiledOrder);
        }

        //批量导入
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.comboxkey.Text != "")
            {
                keys.ImporData(this.comboxkey.Text);
                KeywordManage_Load(sender, e);
                MessageBox.Show("ok");
            }

            //DataEntering data = new DataEntering("关键字录入");

            //if (data.ShowDialog() == DialogResult.OK)
            //{
            //    //刷新数据
            //    KeywordManage_Load(sender, e);
            //}


        }

        //搜索
        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.strWhere = string.Format(" KeyWordsName like '%{0}%'", this.comboxkey.Text);
            FistPage();
        }

        //添加
        private void btnAdd_Click(object sender, EventArgs e)
        {
            KeywordEdit kedit = new KeywordEdit("", "添加");

            if (kedit.ShowDialog() == DialogResult.OK)
            {
                FistPage();
            }
        }

        //编辑
        private void btnEdit_Click(object sender, EventArgs e)
        {
            object obj = this.dataGridKeyWord.CurrentRow;
            if (obj == null)
            {
                return;
            }
            string kid = this.dataGridKeyWord.CurrentRow.Cells[1].Value.ToString();

            KeywordEdit kedit = new KeywordEdit(kid, "编辑");

            if (kedit.ShowDialog() == DialogResult.OK)
            {
                this.strWhere = string.Format(" KID like '%{0}%'", kid);
                FistPage();
            }

        }

        //删除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //删除关键字 需要删除有关的数据记录
            //1.获得一个关键字ID数组
            StringBuilder sbr = new StringBuilder();
            int count = this.dataGridKeyWord.RowCount;
            for (int i = 0; i < count; i++)
            {
                string ob = this.dataGridKeyWord.Rows[i].Cells[0].FormattedValue.ToString();
                if (ob == "True")
                {
                    sbr.Append("'" + this.dataGridKeyWord.Rows[i].Cells[1].Value.ToString() + "'" + ",");
                }
            }

            if (string.IsNullOrEmpty(sbr.ToString()))
            {
                MessageBox.Show("没有选中内容", "消息提示", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string alist = sbr.ToString().Remove(sbr.ToString().Length - 1, 1);
                if (!keys.DeleteList(alist))
                {
                    MessageBox.Show("删除失败！", "消息提示", MessageBoxButtons.OK);
                }
                MessageBox.Show("删除成功", "消息提示", MessageBoxButtons.OK);
            }
            KeywordManage_Load(sender, e);
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



        //首页 分页逻辑算法
        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            FistPage();
        }

        //上一页
        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            this.pageCurrent--;
            BingMovie(this.pageSize, this.pageCurrent, this.strWhere, this.strfiledOrder);
            bindingNavigatorMoveFirstItem.Enabled = true;
            bindingNavigatorMovePreviousItem.Enabled = true;
            if (this.pageCurrent == 1)
            {
                bindingNavigatorMoveFirstItem.Enabled = false;
                bindingNavigatorMovePreviousItem.Enabled = false;
            }
        }

        //下一页
        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            this.pageCurrent++;
            BingMovie(this.pageSize, this.pageCurrent, this.strWhere, this.strfiledOrder);
            bindingNavigatorMoveFirstItem.Enabled = true;
            bindingNavigatorMovePreviousItem.Enabled = true;
            if (this.pageCurrent == this.pageCount)
            {
                bindingNavigatorMoveNextItem.Enabled = false;
                bindingNavigatorMoveLastItem.Enabled = false;
            }
        }

        //文本更改事件
        private void bindingNavigatorPositionItem_TextChanged(object sender, EventArgs e)
        {
            this.btnChoose.Text = "全 选";
        }

        //尾页
        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            LastPage();
        }

        //单元格点击事件
        private void dataGridKeyWord_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //设置选中
            this.dataGridKeyWord.CurrentRow.Cells[0].Value = true;
        }

        //绑定DataGridview
        public void BingMovie(int pageSize, int pageIndex, string strWhere, string strfieldOrder)
        {
            StringBuilder sbr = new StringBuilder();

            if (!string.IsNullOrEmpty(strWhere))
            {
                sbr.Append(strWhere);
            }
            int m;
            this.dtinfo = keys.GetListByPage(pageSize, pageIndex, sbr.ToString(), strfieldOrder, out m);
            dataGridKeyWord.DataSource = dtinfo;

            dataGridKeyWord.Columns["关键字ID"].Visible = false;

            this.bindingNavigator1.Enabled = true;
            bindingNavigatorMoveFirstItem.Enabled = true;
            bindingNavigatorMovePreviousItem.Enabled = true;

            bindingNavigatorMoveNextItem.Enabled = true;
            bindingNavigatorMoveLastItem.Enabled = true;

            bindingNavigatorCountItem.Enabled = true;
            bindingNavigatorPositionItem.Enabled = true;

            this.toolStripLabel1.Text = "总记录：" + m + "条";
            this.bindingNavigator1.PositionItem.Text = pageIndex.ToString();
            this.bindingNavigator1.CountItem.Text = string.Format("/ {0}", this.pageCount.ToString());
        }

        //全选 取消
        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (this.btnChoose.Text.Equals("全 选"))
            {
                for (int i = 0; i < this.dataGridKeyWord.RowCount; i++)
                {
                    //获取第一个单元格的值
                    this.dataGridKeyWord.Rows[i].Cells[0].Value = "true";
                }
                this.btnChoose.Text = "取 消";
            }
            else
            {
                for (int i = 0; i < this.dataGridKeyWord.RowCount; i++)
                {
                    //获取第一个单元格的值
                    this.dataGridKeyWord.Rows[i].Cells[0].Value = "false";
                }
                this.btnChoose.Text = "全 选";
            }
        }

        //应该分页去查询而不是查询之后再进行分页
        #region 1.初始化GridView控件的所有属性

        void InitDataGridView(DataGridView dgv)
        {
            //dgv.AutoGenerateColumns = false;//是否自动创建列
            dgv.AllowUserToAddRows = false;//是否允许添加行(默认：true)
            dgv.AllowUserToDeleteRows = false;//是否允许删除行(默认：true)
            //dgv.AllowUserToResizeColumns = false;//是否允许调整大小(默认：true)
            //dgv.AllowUserToResizeRows = false;//是否允许调整行大小(默认：true)
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//列宽模式(当前填充)(默认：DataGridViewAutoSizeColumnsMode.None)
            //dgv.BackgroundColor = System.Drawing.Color.White;//背景色(默认：ControlDark)
            //dgv.BorderStyle = BorderStyle.Fixed3D;//边框样式(默认：BorderStyle.FixedSingle)
            //dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;//单元格边框样式(默认：DataGridViewCellBorderStyle.Single)
            //dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;//列表头样式(默认：DataGridViewHeaderBorderStyle.Single)
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;//是否允许调整列大小(默认：DataGridViewColumnHeadersHeightSizeMode.EnableResizing)
            //dgv.ColumnHeadersHeight = 30;//列表头高度(默认：20)
            //dgv.MultiSelect = false;//是否支持多选(默认：true)
            dgv.ReadOnly = true;//是否只读(默认：false)
            //dgv.RowHeadersVisible = false;//行头是否显示(默认：true)
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选择模式(默认：DataGridViewSelectionMode.CellSelect)
        }

        #endregion

        //2.初始化各个变量
        private void InitVar()
        {
            this.strWhere = "";
            this.totalCount = 0;
            this.pageCurrent = 1;
            this.pageCount = 0;
            this.pageSize = 25;
        }

        //3.计算页数
        public void CountPage()
        {
            //计算出当前的总页数 和总条数
            this.totalCount = keys.GetRecordCount(this.strWhere);

            if (this.totalCount % this.pageSize == 0)
            {
                this.pageCount = this.totalCount / this.pageSize;
            }
            else
            {
                this.pageCount = (this.totalCount / this.pageSize) + 1;
            }

        }

        //4.绑定首页
        public void FistPage()
        {
            this.pageCurrent = 1;
            BingMovie(this.pageSize, 1, this.strWhere, this.strfiledOrder);
            bindingNavigatorMoveFirstItem.Enabled = false;
            bindingNavigatorMovePreviousItem.Enabled = false;
        }

        //5.绑定尾页
        public void LastPage()
        {
            this.pageCurrent = this.pageCount;
            BingMovie(this.pageSize, this.pageCount, this.strWhere, this.strfiledOrder);

            bindingNavigatorMoveFirstItem.Enabled = true;
            bindingNavigatorMovePreviousItem.Enabled = true;

            bindingNavigatorMoveNextItem.Enabled = false;
            bindingNavigatorMoveLastItem.Enabled = false;
        }

        //废弃代码
        public void LoadKeyWordsData()
        {
            //BindingSource bs = new BindingSource();
            //bs.DataSource = keys.GetList("").Tables[0];
            //bindingNavigator1.BindingSource = bs;
            //this.dataGridKeyWord.DataSource = bs;
        }
    }
}
