using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlTypes;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private SqlConnection GetSqlConnection()
        {
            SqlConnection c = new SqlConnection();
            c.ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Database=WebStore;Trusted_Connection=yes;";
            return c;
        }
    
        public bool CreateProduct(string key,string price)
        {
            string a = "12.3";
            SqlMoney b = SqlMoney.Parse(a.Replace(',','.'));
           // string a = "15,2";
            //price1 = Convert.ToDouble(a.Replace('.', ','));
            //Console.WriteLine(price1);
            //string l = (price.GetType()).ToString();
            //double a = Convert.ToDouble(price);
            var c = GetSqlConnection();
            c.Open();
            var cmd = c.CreateCommand();
            cmd.CommandText = $"INSERT into Product values('{key}', 'L', 'Blue',{b}, 'Dress')";
            var result = cmd.ExecuteNonQuery();
            c.Close();
            return result > 0;
        }
    }
}
