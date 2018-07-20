// ================================================================================
// 		File: ExcelVersion.cs
// 		Desc: ���ڱ�ʶExcel�汾�ţ�ͬʱ��appconfig�ļ��еļ�ֵ��Ӧ��
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
using System.Text;

namespace xiaoy.Excel
{
    /// <summary>
    /// Excel�ĵ��汾���͡�
    /// ���� Microsoft Excel 12.0 (2007) ����������ʹ�� Excel 12.0�� 
    /// ���� Microsoft Excel 8.0 (97)��9.0 (2000) �� 10.0 (2002) ����������ʹ�� Excel 8.0�� 
    /// ���� Microsoft Excel 5.0 �� 7.0 (95) ����������ʹ�� Excel 5.0�� 
    /// ���� Microsoft Excel 4.0 ����������ʹ�� Excel 4.0�� 
    /// ���� Microsoft Excel 3.0 ����������ʹ�� Excel 3.0��
    /// </summary>
    public class ExcelVersion
    {
        /// <summary>
        /// Excel3.0���ĵ���ʽ
        /// </summary>
        public static string Excel3 = "Excel3.0";
        /// <summary>
        /// Excel4.0���ĵ���ʽ
        /// </summary>
        public static string Excel4 = "Excel4.0";
        /// <summary>
        /// Excel5.0���ĵ���ʽ�������� Microsoft Excel 5.0 �� 7.0 (95) ������
        /// </summary>
        public static string Excel5 = "Excel5.0";
        /// <summary>
        /// Excel8.0���ĵ���ʽ��������Microsoft Excel 8.0 (98-2003) ������
        /// </summary>
        public static string Excel8 = "Excel8.0";
        /// <summary>
        /// Excel12.0���ĵ���ʽ��������Microsoft Excel 12.0 (2007) ������
        /// </summary>
        public static string Excel12 = "Excel12.0";
    }
}
