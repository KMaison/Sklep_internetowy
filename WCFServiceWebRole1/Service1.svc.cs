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
    public class Service1 : IService1
    {
        private SqlConnection GetSqlConnection()
        {
            SqlConnection c = new SqlConnection();
            c.ConnectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Database=WebStore;Trusted_Connection=true;";
            try
            {
                c.Open();
            }
            catch (Exception e)
            {
                return c; //todo
            }
            return c;
        }

        public bool AddProduct(string key, string size, string color, string price, string type)
        {
            string query = "INSERT INTO Product " +
                "(Bar_code, Size, Color, Price, Clothes_type)";
            query += " VALUES (@Bar_code, @Size, @Color, @Price, @Clothes_type)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection); 
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            try
            {
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                myConnection.Close();
            }
            return true;
        }
    }
}
