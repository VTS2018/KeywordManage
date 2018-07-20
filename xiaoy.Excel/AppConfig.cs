// ================================================================================
// 		File: AppConfig.cs
// 		Desc: �����ļ��Ķ�ȡ����ȡweb.config��app.config�������ļ������ݡ�
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
    /// ϵͳ�����࣬��ȡϵͳ���õ����ò���
    /// ����ʵ����IConfigurationSectionHandler�ӿ�
    /// </summary>
    public class AppConfig : System.Configuration.IConfigurationSectionHandler
    {
        #region ˽�б���

        private string m_strSectionName;

        /// <summary>
        /// ����Ӧ��ϵͳ���öΣ��ڸ�Ӧ��ϵͳ��ʵ������
        /// </summary>
        private NameValueCollection m_configValues;

        /// <summary>
        /// ���õļ�ֵ��
        /// </summary>
        private static NameValueCollection m_configApp;

        #endregion

        #region  ���캯��
        /// <summary>
        /// ��ʼ�� AppConfiguration �����ʵ����
        /// </summary>
        public AppConfig()
            : this(null)
        {
        }
        /// <summary>
        /// ��ʼ�� AppConfiguration �����ʵ����
        /// </summary>
        /// <param name="sectionName">Ĭ�϶�ȡ�Ķε����ơ�</param>
        public AppConfig(string sectionName)
        {
            m_strSectionName = sectionName;
        }
        #endregion

        #region ����
        /// <summary>
        /// ��ȡ���������ö�����
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
        /// ��ȡappSettings���ö�������ݡ�	<add key="MaxSheelSize" value="80000"/>	
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
        /// ��ȡ�������û�ָ�����ö�������ݺ�Ĭ�����ݵĲ�����Ĭ��ΪappSettings������ݣ���
        /// </summary>
        public virtual NameValueCollection ModuleConfigSettings
        {
            get
            {
                if (m_configValues == null)
                {
                    m_configValues = new NameValueCollection();
                    //���AppSetting����
                    m_configValues.Add(AppSettings);
                    if (SectionName != null && !SectionName.Equals(string.Empty))
                    {
                        NameValueCollection nv = GetConfig(SectionName);
                        //��ģ�����ò�Ϊ��ʱ�����ģ������
                        if (nv != null)
                        {
                            //ģ�����ÿ��Ը���AppSetting�е�����
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
        /// ��װ��һ��, ȷ�����ñ������ķ���ΪDll�ڲ�����, �Ӷ�ȡ����ȷ��Dll�����ļ�·��
        /// �������ȡ�õ���ִ�г���(������)��·��
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

        #region ����
        /// <summary>
        ///  ��ȡĳ�������е�������Ϣ��Ϊ����ֵ�����öΣ���
        /// </summary>
        /// <param name="sectionName">���öε�����Z</param>
        /// <returns>���ö��е�����ֵ�ԡ�</returns>
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
        /// ��ȡdll��Ӧ������
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

        #region IConfigurationSectionHandler ��Ա

        /// <summary>
        /// �����µ����ô�����򲢽�����ӵ��ڴ�����򼯺��С�
        /// </summary>
        /// <param name="parent">��Ӧ�����ý��е��������á�</param>
        /// <param name="configContext">���ýڴ������Ϊ���������ֵ������·����ͨ�����ò����Ǳ�����������Ϊ�����ã�Visual Basic ��Ϊ Nothing���� </param>
        /// <param name="section">����Ҫ�����������Ϣ�� XmlNode���ṩ�����ý� XML ���ݵ�ֱ�ӷ��ʡ�</param>
        /// <returns>һ�� NameValueCollection��</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            NameValueSectionHandler handler = new NameValueSectionHandler();
            return handler.Create(parent, configContext, section);
        }

        #endregion
    }
}
