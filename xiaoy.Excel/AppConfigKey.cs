// ================================================================================
// 		File: AppConfigKey.cs
// 		Desc: �����ļ��еļ�ֵ��
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
    /// �����ļ������ýڵ������
    /// </summary>
    public class AppConfigKey
    {
        /// <summary>
        /// Excel�汾���ü�
        /// </summary>
        public const string ExcelVersionKey = "ExcelVersion";

        /// <summary>
        /// Excel���Ͷ��ձ����ü�
        /// </summary>
        public const string ExcelTypeKey = "ExcelTypeMap";

        /// <summary>
        /// Excel���Ͷ��ձ��е�Ĭ���������ü�
        /// </summary>
        public const string DefaultTypeKey = "Default";

        /// <summary>
        /// Ĭ��ÿ��Excelҳ�пɴ����������������ü�
        /// </summary>
        public const string MaxSheelSize = "MaxSheelSize";
    }
}
