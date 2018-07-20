using System;
namespace KM.Entity
{
    /// <summary>
    /// KeyWords:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class KeyWords
    {
        public KeyWords()
        { }
        #region Model
        private string _kid;
        private string _keywordsname;
        private string _keywordsstatus;
        /// <summary>
        /// 
        /// </summary>
        public string KID
        {
            set { _kid = value; }
            get { return _kid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string KeyWordsName
        {
            set { _keywordsname = value; }
            get { return _keywordsname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string KeyWordsStatus
        {
            set { _keywordsstatus = value; }
            get { return _keywordsstatus; }
        }
        #endregion Model
    }
}
