// ================================================================================
// 		File: HDRType.cs
// 		Desc: HDRType类,用于标识Excel第一行是否标题。
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
    /// HDR类型，用于标识第一行是否标题。
    /// </summary>
    public class HDRType
    {
        /// <summary>
        /// HDR=Yes，这代表第一行是标题，不做为数据使用
        /// </summary>
        public static string Yes = "YES";
        /// <summary>
        /// HDR=NO，则表示第一行不是标题，做为数据来使用
        /// </summary>
        public static string No = "NO";
    }
}
