using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Client.Domain;
namespace WCFServiceWebRole1
{
    public class Service1 : IService1
    {
        private SqlConnection GetSqlConnection()
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

        public bool AddOrderProduct(string amount, string bar_code, string id_client_order)
        {
            string query = "INSERT INTO Order_products " +
                "(Amount, Bar_code,ID_client_order)";
            query += " VALUES (@Amount,@Bar_code,@ID_client_order)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Amount", amount);
            myCommand.Parameters.AddWithValue("@Bar_code", bar_code);
            myCommand.Parameters.AddWithValue("@ID_client_order", id_client_order);

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

        public bool AddClient(string first_name, string surname, string order_id)
        {
            string query = "INSERT INTO Client " +
                "(Firstname,Surname,Order_ID)";
            query += " VALUES (@Firstname,@Surname,@Order_ID)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
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

        public bool AddClientOrder(string orderid, string address, string order_status)
        {
            string query = "INSERT INTO CLient_order " +
                "(Order_ID,Adress, Order_status)";
            query += " VALUES ( @Order_ID,@Adress, @Order_status)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Order_ID", orderid);
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

        public bool AddProduct(string key, string name, string size, string color, string price, string type, string amount)
        {
            if (price.Contains(","))
                price.Replace(',', '.');

            string query = "INSERT INTO Product " +
                "(Bar_code,Name, Size, Color, Price, Clothes_type,Amount)";
            query += " VALUES (@Bar_code,@Name, @Size, @Color, @Price, @Clothes_type,@Amount)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Name", name);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            myCommand.Parameters.AddWithValue("@Amount", amount);
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

        public bool UpdateProduct(string key, string size, string color, string price, string type, string amount)
        {
            string query = "UPDATE Product SET Size = @Size, Color = @Color, Price = @Price, Clothes_type = @Clothes_type, Amount=@Amount WHERE Bar_code = @Bar_code";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            myCommand.Parameters.AddWithValue("@Amount", amount);
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

        public bool UpdateOrderProduct(string id, string amount, string bar_code)
        {
            string query = "UPDATE Order_products SET Amount = @Amount,Bar_code = @Bar_code  WHERE ID_order_product=@ID_order_product ";

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

        public bool UpdateClient(string pesel, string first_name, string surname, string order_id)
        {
            string query = "UPDATE Client SET Firstname = @Firstname, Surname = @Surname, Order_ID=@Order_ID  WHERE PESEL=@PESEL ";

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

        public bool UpdateClientOrder(string order_id, string id_order_product, string address, string order_status)
        {
            string query = "UPDATE Client_order SET ID_order_product = @ID_order_product, Adress = @Adress, Order_status=@Order_status  WHERE Order_ID=@Order_ID ";

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

        public String[] SetProductList()
        {
            string query = "SELECT * FROM Product";
            SqlDataReader myreader;

            SqlConnection myConnection = GetSqlConnection();
            String[] productList = new String[countProduct()];
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            try
            {
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return null; //TODO
            }
            finally
            {
                int i = 0;
                myreader = myCommand.ExecuteReader();
                while (myreader.Read())
                {
                    String tmp = myreader[0].ToString() + ";" +
                        myreader[1].ToString() + ";" +
                        myreader[2].ToString() + ";" +
                        myreader[3].ToString() + ";" +
                        myreader[4].ToString() + ";" +
                        myreader[5].ToString() + ";" +
                        myreader[6].ToString();
                    productList[i] = tmp;
                    i++;

                }
            }
            myConnection.Close();
            return productList;
        }

        public bool ifProductExist(string size, string color, string type)
        {
            string query = "SELECT p.Name FROM Product p WHERE (p.Size=@Size AND p.Color=@Color AND p.Clothes_type=@Type) ";

            SqlConnection myConnection = GetSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            SqlDataReader myreader;

            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Type", type);
            try
            {
                myCommand.ExecuteNonQuery();
                myreader = myCommand.ExecuteReader();
                myreader.Read();
                var c = myreader[0];
            }
            catch (Exception e)
            {
                return false;
            }
            myConnection.Close();

            return true;
        }
        public bool ifProductAmountEnough(string id, string amount)
        {
            string query = "SELECT p.Name FROM Product p WHERE (p.Bar_code=@Bar_code AND p.Amount>=@Amount) ";

            SqlConnection myConnection = GetSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            SqlDataReader myreader;
            myCommand.Parameters.AddWithValue("@Bar_code", id);
            myCommand.Parameters.AddWithValue("@Amount", amount);
            try
            {
                myCommand.ExecuteNonQuery();
                myreader = myCommand.ExecuteReader();
                myreader.Read();
                var c = myreader[0];
            }
            catch (Exception e)
            {
                return false;
            }
            myConnection.Close();

            return true;
        }

        private int countProduct()
        {
            string query = "SELECT COUNT(p.Name) FROM Product p  ";

            SqlConnection myConnection = GetSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            SqlDataReader myreader;
            int c;
            try
            {
                myCommand.ExecuteNonQuery();
                myreader = myCommand.ExecuteReader();
                myreader.Read();
                c = Int32.Parse(myreader[0].ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
            myConnection.Close();

            return c;

        }

        public int CreateClientOrder(string address)
        {

            string status = "Processing";
            int id=0;
            bool correct = false;
            while (!correct)
            {
                id = new Random().Next(100, 9999999);

                string query = "SELECT p.Order_ID FROM Client_order p WHERE (p.Order_ID=@id) ";

                SqlConnection myConnection = GetSqlConnection();
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
            
            AddClientOrder(id.ToString(), address, status);


            return id;
        }
    }
}
