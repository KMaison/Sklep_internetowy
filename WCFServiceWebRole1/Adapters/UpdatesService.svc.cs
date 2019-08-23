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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UpdatesService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UpdatesService.svc or UpdatesService.svc.cs at the Solution Explorer and start debugging.
    public class UpdatesService : UpdatesInterface
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

        public bool updateClient(string pesel, string first_name, string surname, string order_id)
        {
            string query = "UPDATE Client SET Firstname = @Firstname, Surname = @Surname, Order_ID=@Order_ID  WHERE PESEL=@PESEL ";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@PESEL", pesel);
            myCommand.Parameters.AddWithValue("@Firstname", first_name);
            myCommand.Parameters.AddWithValue("@Surname", surname);
            myCommand.Parameters.AddWithValue("@Order_ID", order_id);

            return runQuery(myConnection, myCommand);
        }

        public bool updateClientOrder(string order_id, string id_order_product, string address, string order_status)
        {
            string query = "UPDATE Client_order SET ID_order_product = @ID_order_product, Adress = @Adress, Order_status=@Order_status  WHERE Order_ID=@Order_ID ";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Order_ID", order_id);
            myCommand.Parameters.AddWithValue("@ID_order_product", id_order_product);
            myCommand.Parameters.AddWithValue("@Adress", address);
            myCommand.Parameters.AddWithValue("@Order_status", order_status);

            return runQuery(myConnection, myCommand);
        }

        public bool updateOrderProduct(string id, string amount, string bar_code)
        {
            string query = "UPDATE Order_products SET Amount = @Amount,Bar_code = @Bar_code  WHERE ID_order_product=@ID_order_product ";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@ID_order_product", id);
            myCommand.Parameters.AddWithValue("@Amount", amount);
            myCommand.Parameters.AddWithValue("@Bar_code", bar_code);

            return runQuery(myConnection, myCommand);
        }

        public bool updateProduct(string key, string size, string color, string price, string type, string amount_Reserved, string amount_To_Reserve)
        {
            string query = "UPDATE Product SET ";
            query += "Size = @Size, Color = @Color, Price = @Price, Clothes_type = @Clothes_type, Amount_Reserved= @Amount_Reserved, Amount_To_Reserve= @Amount_To_Reserve";
            query += " WHERE Bar_code = @Bar_code";

            SqlConnection myConnection = getSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            myCommand.Parameters.AddWithValue("@Amount_Reserved", amount_Reserved);
            myCommand.Parameters.AddWithValue("@Amount_To_Reserve", amount_To_Reserve);

            return runQuery(myConnection, myCommand);
        }
    }
}
