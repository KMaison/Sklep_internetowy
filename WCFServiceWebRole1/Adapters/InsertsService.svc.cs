using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFServiceWebRole1.Ports;

namespace WCFServiceWebRole1.Adapters
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InsertsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select InsertsService.svc or InsertsService.svc.cs at the Solution Explorer and start debugging.
    public class InsertsService : InsertsInterface
    {
        private SqlConnection getSqlConnection()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=WebStore;Trusted_Connection=true;"
            };
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                return sqlConnection; //todo
            }
            return sqlConnection;
        }

        private bool runQuery(SqlConnection myConnection, SqlCommand myCommand)
        {
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

        public bool addOrderProduct(string amount, string bar_code, string id_client_order)
        {
            string query = "INSERT INTO Order_products " +
                 "(Amount, Bar_code,ID_client_order)";
            query += " VALUES (@Amount,@Bar_code,@ID_client_order)";

            SqlConnection myConnection = getSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Amount", amount);
            myCommand.Parameters.AddWithValue("@Bar_code", bar_code);
            myCommand.Parameters.AddWithValue("@ID_client_order", id_client_order);

            return runQuery(myConnection, myCommand);
        }

        public bool addProduct(string key, string name, string size, string color, string price, string type, string amount)
        {
            if (price.Contains(","))
                price.Replace(',', '.');

            string query = "INSERT INTO Product " +
                "(Bar_code,Name, Size, Color, Price, Clothes_type,Amount_Reserved,Amount_To_Reserve)";
            query += " VALUES (@Bar_code,@Name, @Size, @Color, @Price, @Clothes_type,@Amount_Reserved,@Amount_To_Reserve)";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Name", name);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            myCommand.Parameters.AddWithValue("@Amount_Reserved", 0);
            myCommand.Parameters.AddWithValue("@Amount_To_Reserve", amount);

            return runQuery(myConnection, myCommand);
        }

        public bool addClient(string first_name, string surname, string order_id)
        {
            string query = "INSERT INTO Client " +
                "(Firstname,Surname,Order_ID)";
            query += " VALUES (@Firstname,@Surname,@Order_ID)";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Firstname", first_name);
            myCommand.Parameters.AddWithValue("@Surname", surname);
            myCommand.Parameters.AddWithValue("@Order_ID", order_id);

            return runQuery(myConnection, myCommand);
        }

        public bool addClientOrder(string orderid, string address, string order_status)
        {
            string query = "INSERT INTO CLient_order " +
                 "(Order_ID,Adress, Order_status)";
            query += " VALUES ( @Order_ID,@Adress, @Order_status)";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Order_ID", orderid);
            myCommand.Parameters.AddWithValue("@Adress", address);
            myCommand.Parameters.AddWithValue("@Order_status", order_status);

            return runQuery(myConnection, myCommand);
        }

        public int createClientOrder(string address)
        {
            string status = "Processing";
            int id = 0;
            bool correct = false;
            while (!correct)
            {
                id = new Random().Next(100, 9999999);

                string query = "SELECT p.Order_ID FROM Client_order p WHERE (p.Order_ID=@id) ";

                SqlConnection myConnection = getSqlConnection();
                SqlCommand myCommand = new SqlCommand(query, myConnection);
                SqlDataReader myreader;
                myCommand.Parameters.AddWithValue("@id", id);
                try
                {
                    myCommand.ExecuteNonQuery();
                    myreader = myCommand.ExecuteReader();
                    myreader.Read();
                    var c = myreader[0];
                }
                catch
                {
                    correct = true;
                }
                myConnection.Close();
            }

            addClientOrder(id.ToString(), address, status);

            return id;
        }
    }
}
