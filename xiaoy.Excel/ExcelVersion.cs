// ================================================================================
// 		File: ExcelVersion.cs
// 		Desc: 用于标识Excel版本号，同时与appconfig文件中的键值对应。
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
    /// Excel文档版本类型。
    /// 对于 Microsoft Excel 12.0 (2007) 工作簿，请使用 Excel 12.0。 
    /// 对于 Microsoft Excel 8.0 (97)、9.0 (2000) 和 10.0 (2002) 工作簿，请使用 Excel 8.0。 
    /// 对于 Microsoft Excel 5.0 和 7.0 (95) 工作簿，请使用 Excel 5.0。 
    /// 对于 Microsoft Excel 4.0 工作簿，请使用 Excel 4.0。 
    /// 对于 Microsoft Excel 3.0 工作簿，请使用 Excel 3.0。
    /// </summary>
    public class ExcelVersion
    {
        /// <summary>
        /// Excel3.0版文档格式
        /// </summary>
        public static string Excel3 = "Excel3.0";
        /// <summary>
        /// Excel4.0版文档格式
        /// </summary>
        public static string Excel4 = "Excel4.0";
        /// <summary>
        /// Excel5.0版文档格式，适用于 Microsoft Excel 5.0 和 7.0 (95) 工作簿
        /// </summary>
        public static string Excel5 = "Excel5.0";
        /// <summary>
        /// Excel8.0版文档格式，适用于Microsoft Excel 8.0 (98-2003) 工作簿
        /// </summary>
        public static string Excel8 = "Excel8.0";
        /// <summary>
        /// Excel12.0版文档格式，适用于Microsoft Excel 12.0 (2007) 工作簿
        /// </summary>
        public static string Excel12 = "Excel12.0";
    }
}
