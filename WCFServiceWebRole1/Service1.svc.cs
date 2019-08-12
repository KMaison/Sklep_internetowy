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
            try{
                c.Open();
            }
            catch (Exception e){
                return c; //todo
            }
            return c;
        }
        public bool AddOrderProduct(string id,string amount, string bar_code)
        {
            string query = "INSERT INTO Order_products " +
                "( Amount, Bar_code)";
            query += " VALUES (@Amount,@Bar_code)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@ID_order_product", id);
            myCommand.Parameters.AddWithValue("@Amount", amount);
            myCommand.Parameters.AddWithValue("@Bar_code", bar_code);
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

        public bool AddClient(string pesel, string first_name,string surname, string order_id)
        {
            string query = "INSERT INTO Client " +
                "( PESEL,Firstname,Surname,Order_ID)";
            query += " VALUES ( @PESEL,@Firstname,@Surname,@Order_ID)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@PESEL", pesel);
            myCommand.Parameters.AddWithValue("@Firstname", first_name);
            myCommand.Parameters.AddWithValue("@Surname", surname);
            myCommand.Parameters.AddWithValue("@Order_ID", order_id);
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

        public bool AddClientOrder(string order_id, string id_order_product, string address, string order_status)
        {
            string query = "INSERT INTO CLient_order " +
                "(Order_ID, ID_order_product,Adress, Order_status)";
            query += " VALUES (@Order_ID, @ID_order_product, @Adress, @Order_status)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Order_ID", order_id);
            myCommand.Parameters.AddWithValue("@ID_order_product", id_order_product);
            myCommand.Parameters.AddWithValue("@Adress", address);
            myCommand.Parameters.AddWithValue("@Order_status", order_status);
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
            try {
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e){
                return false;
            }
            finally{
                myConnection.Close();
            }
            return true;
        }

        public bool UpdateProduct(string key, string size, string color, string price, string type)
        {
            string query = "UPDATE Product SET Size = @Size, Color = @Color, Price = @Price, Clothes_type = @Clothes_type WHERE Bar_code = @Bar_code";

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
