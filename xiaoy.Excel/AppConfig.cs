// ================================================================================
// 		File: AppConfig.cs
// 		Desc: 配置文件的读取。读取web.config，app.config等配置文件的内容。
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Configuration;

namespace xiaoy.Excel
{
    /// <summary>
    /// 系统配置类，获取系统配置的配置参数
    /// 此类实现了IConfigurationSectionHandler接口
    /// </summary>
    public class AppConfig : System.Configuration.IConfigurationSectionHandler
    {
        #region 私有变量

        private string m_strSectionName;

        /// <summary>
        /// 设置应用系统配置段，在各应用系统中实例化。
        /// </summary>
        private NameValueCollection m_configValues;

        /// <summary>
        /// 配置的键值对
        /// </summary>
        private static NameValueCollection m_configApp;

        #endregion

        #region  构造函数
        /// <summary>
        /// 初始化 AppConfiguration 类的新实例。
        /// </summary>
        public AppConfig()
            : this(null)
        {
        }
        /// <summary>
        /// 初始化 AppConfiguration 类的新实例。
        /// </summary>
        /// <param name="sectionName">默认读取的段的名称。</param>
        public AppConfig(string sectionName)
        {
            m_strSectionName = sectionName;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置配置段名。
        /// </summary>
        protected virtual string SectionName
        {
            get
            {
                return m_strSectionName;
            }
            set
            {
                m_strSectionName = value;
            }
        }

        /// <summary>
        /// 获取appSettings配置段里的内容。	<add key="MaxSheelSize" value="80000"/>	
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get
            {
                if (m_configApp == null)
                {
                    m_configApp = new NameValueCollection();
                    //NameValueCollection v = (NameValueCollection)ConfigurationSettings.AppSettings;
                    KeyValueConfigurationCollection v = GetDllConfiguration().AppSettings.Settings;
                    if (v != null)
                    {
                        foreach (string key in v.AllKeys)
                        {
                            m_configApp.Set(key, v[key].Value);
                        }
                    }
                }
                return m_configApp;
            }
        }

        /// <summary>
        /// 获取或设置用户指定配置段里的内容和默认内容的并集（默认为appSettings里的内容）。
        /// </summary>
        public virtual NameValueCollection ModuleConfigSettings
        {
            get
            {
                if (m_configValues == null)
                {
                    m_configValues = new NameValueCollection();
                    //添加AppSetting配置
                    m_configValues.Add(AppSettings);
                    if (SectionName != null && !SectionName.Equals(string.Empty))
                    {
                        NameValueCollection nv = GetConfig(SectionName);
                        //当模块配置不为空时，添加模块配置
                        if (nv != null)
                        {
                            //模块配置可以覆盖AppSetting中的配置
                            foreach (string key in nv.Keys)
                            {
                                m_configValues.Set(key, nv[key]);
                            }
                        }
                    }
                }
                return m_configValues;
            }
            set
            {
                m_configValues = value;
            }
        }

        /// <summary>
        /// 封装多一遍, 确保调用本方法的方法为Dll内部方法, 从而取得正确的Dll配置文件路径
        /// 否则可能取得的是执行程序(主程序)的路径
        /// </summary>
        private static string DllConfigFilePath
        {
            get
            {
                Assembly t_assembly = Assembly.GetCallingAssembly();

                Uri t_uri = new Uri(Path.GetDirectoryName(t_assembly.CodeBase));

                return Path.Combine(t_uri.LocalPath, t_assembly.GetName().Name + ".config");
            }
        }

        #endregion

        #region 方法
        /// <summary>
        ///  获取某段配置中的配置信息（为名称值的配置段）。
        /// </summary>
        /// <param name="sectionName">配置段的名称Z</param>
        /// <returns>配置段中的名称值对。</returns>
        public static NameValueCollection GetConfig(string sectionName)
        {
            NameValueCollection values = null;
            try
            {
                values = new NameValueCollection();
                ConfigurationSection cs = GetDllConfiguration().GetSection(sectionName);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(cs.SectionInformation.GetRawXml());

                XmlNode xList = xDoc.ChildNodes[0];
                foreach (XmlNode xNode in xList.ChildNodes)
                {
                    if (xNode.NodeType == XmlNodeType.Element)
                    {
                        values.Add(xNode.Attributes[0].Value, xNode.Attributes[1].Value);
                    }
                }
            }
            catch
            {
                values = new NameValueCollection();
            }
            return values;
        }

        /// <summary>
        /// 获取dll对应的配置
        /// </summary>
        /// <param name="targetAsm"></param>
        /// <returns></returns>
        private static Configuration GetDllConfiguration()
        {
            string configFile = DllConfigFilePath;
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFile;
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        #endregion

        #region IConfigurationSectionHandler 成员

        /// <summary>
        /// 创建新的配置处理程序并将其添加到节处理程序集合中。
        /// </summary>
        /// <param name="parent">对应父配置节中的配置设置。</param>
        /// <param name="configContext">配置节处理程序为其计算配置值的虚拟路径。通常，该参数是保留参数，并为空引用（Visual Basic 中为 Nothing）。 </param>
        /// <param name="section">包含要处理的配置信息的 XmlNode。提供对配置节 XML 内容的直接访问。</param>
        /// <returns>一个 NameValueCollection。</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            NameValueSectionHandler handler = new NameValueSectionHandler();
            return handler.Create(parent, configContext, section);
        }

        #endregion
    }
}
