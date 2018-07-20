// ================================================================================
// 		File: AppConfigKey.cs
// 		Desc: 配置文件中的键值。
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
using System.Text;

namespace xiaoy.Excel
{
    /// <summary>
    /// 配置文件中配置节点的名称
    /// </summary>
    public class AppConfigKey
    {
        /// <summary>
        /// Excel版本配置键
        /// </summary>
        public const string ExcelVersionKey = "ExcelVersion";

        /// <summary>
        /// Excel类型对照表配置键
        /// </summary>
        public const string ExcelTypeKey = "ExcelTypeMap";

        /// <summary>
        /// Excel类型对照表中的默认类型配置键
        /// </summary>
        public const string DefaultTypeKey = "Default";

        /// <summary>
        /// 默认每个Excel页中可存的最大数据条数配置键
        /// </summary>
        public const string MaxSheelSize = "MaxSheelSize";
    }
}
