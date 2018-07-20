using System;
using System.Configuration;

namespace KM.DBUtility
{
    public class PubConStant
    {
        public static string ConnecnString
        {
            get
            {
                string Connstring = ConfigurationManager.ConnectionStrings["KMConnentString"].ConnectionString;
                //string Connstring = @"Data Source=50.115.134.43\SQLEXPRESS,1444;Initial Catalog=SNData_Company01;User Id=sa; PassWord=hockeyoff123456789zx";
                //string Connstring = @"server=.\SQLEXPRESS;uid=sa;pwd=sa;database=SData0527;";
                //string Connstring = @"server=VPS-606BA1B317E\SQLEXPRESS;database=SNData_Company01;uid=sa; pwd=hockeyoff123456789zx";
                return Connstring;
            }
        }
    }
}
