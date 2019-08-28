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
        private bool RunQuery(SqlConnection myConnection, SqlCommand myCommand)
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


            return RunQuery(myConnection, myCommand);
        }

        public bool AddClient(string first_name, string surname, string order_id)
        {
            string query = "INSERT INTO Client " +
                "(Firstname,Surname,ID_client_order)";
            query += " VALUES (@Firstname,@Surname,@ID_client_order)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Firstname", first_name);
            myCommand.Parameters.AddWithValue("@Surname", surname);

            myCommand.Parameters.AddWithValue("@ID_client_order", order_id);

            return RunQuery(myConnection, myCommand);
        }

        public int CreateClientOrder(string address)
        {

            string status = "Processing";
            int id = 0;
            bool correct = false;
            while (!correct)
            {
                id = new Random().Next(100, 9999999);

                string query = "SELECT p.Order_ID FROM Client_order p WHERE (p.ID_client_order=@ID_client_order) ";

                SqlConnection myConnection = GetSqlConnection();
                SqlCommand myCommand = new SqlCommand(query, myConnection);
                SqlDataReader myreader;
                myCommand.Parameters.AddWithValue("@ID_client_order", id);
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

            bool x = AddClientOrder(id.ToString(), address, status);
            return id;
        }

        public bool AddClientOrder(string orderid, string address, string order_status)
        {
            string query = "INSERT INTO Client_order " +
                "(ID_client_order,Adress, Order_status)";
            query += " VALUES ( @ID_client_order,@Adress, @Order_status)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@ID_client_order", orderid);
            myCommand.Parameters.AddWithValue("@Adress", address);
            myCommand.Parameters.AddWithValue("@Order_status", order_status);

            return RunQuery(myConnection, myCommand);
        }

        public bool AddProduct(string name, string size, string color, string price, string type, string amount)
        {
            if (price.Contains(","))
                price.Replace(',', '.');

            string query = "INSERT INTO Product " +
                "(Name, Size, Color, Price, Clothes_type,Amount_Reserved,Amount_To_Reserve)";
            query += " VALUES (@Name, @Size, @Color, @Price, @Clothes_type,@Amount_Reserved,@Amount_To_Reserve)";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Name", name);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            myCommand.Parameters.AddWithValue("@Amount_Reserved", 0);
            myCommand.Parameters.AddWithValue("@Amount_To_Reserve", amount);

            return RunQuery(myConnection, myCommand);
        }

        public bool UpdateProduct(string key, string size, string color, string price, string type, string amount_Reserved, string amount_To_Reserve)//czy ta metoda wgl jest potrzebna??
        {
            string query = "UPDATE Product SET ";
            query += "Size = @Size, Color = @Color, Price = @Price, Clothes_type = @Clothes_type, Amount_Reserved= @Amount_Reserved, Amount_To_Reserve= @Amount_To_Reserve";
            query += " WHERE Bar_code = @Bar_code";

            SqlConnection myConnection = GetSqlConnection();

            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Size", size);
            myCommand.Parameters.AddWithValue("@Color", color);
            myCommand.Parameters.AddWithValue("@Price", price);
            myCommand.Parameters.AddWithValue("@Clothes_type", type);
            myCommand.Parameters.AddWithValue("@Amount_Reserved", amount_Reserved);
            myCommand.Parameters.AddWithValue("@Amount_To_Reserve", amount_To_Reserve);

            return RunQuery(myConnection, myCommand);
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

            return RunQuery(myConnection, myCommand);
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
            myCommand.Parameters.AddWithValue("@Order_ID", order_id);

            return RunQuery(myConnection, myCommand);
        }

        public String[] GetProductList()
        {
            string query = "SELECT * FROM Product";
            SqlDataReader myreader;

            SqlConnection myConnection = GetSqlConnection();
            String[] productList = new String[CountProduct()];
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
                        myreader[6].ToString() + ";" +
                        myreader[7].ToString();
                    productList[i] = tmp;
                    i++;

                }
            }
            myConnection.Close();
            return productList;
        }

        public bool ifProductAmountEnough(string id, string amount)
        {
            string query = "SELECT p.Name FROM Product p WHERE (p.Bar_code=@Bar_code AND p.Amount_To_Reserve>=@Amount) ";

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
       
        public string getProductPrice(string id)
        {
            string query = "SELECT p.Price FROM Product p WHERE (p.Bar_code=@Bar_code) ";

            SqlConnection myConnection = GetSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            SqlDataReader myreader;
            var c = new Object();

            myCommand.Parameters.AddWithValue("@Bar_code", id);
            try
            {
                myCommand.ExecuteNonQuery();
                myreader = myCommand.ExecuteReader();
                myreader.Read();
                c = myreader[0];
            }
            catch (Exception e)
            {
                return null;
            }
            myConnection.Close();

            return c.ToString();
        }
        
        private int CountProduct()

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

        public bool ReserveProduct(string key, string amount)
        {
            string query = "UPDATE Product SET ";
            query += "Amount_Reserved= @Amount_Reserved, Amount_To_Reserve= @Amount_To_Reserve";
            query += " WHERE Bar_code = @Bar_code";

            SqlConnection myConnection = GetSqlConnection();

            int amount_To_Reserve = GetAmount_To_Reserve(key) - Int32.Parse(amount);
            int amount_reserved = GetAmount_Reserved(key) + Int32.Parse(amount);
            if (amount_To_Reserve < 0 || amount_reserved < 0)
                return false;
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Amount_Reserved", amount_reserved);
            myCommand.Parameters.AddWithValue("@Amount_To_Reserve", amount_To_Reserve);
            return RunQuery(myConnection, myCommand);
        }


        int GetAmount_To_Reserve(string id)
        {
            string query = "SELECT p.Amount_To_Reserve FROM Product p WHERE (p.Bar_code=@Bar_code)";

            SqlConnection myConnection = GetSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            SqlDataReader myreader;
            myCommand.Parameters.AddWithValue("@Bar_code", Int32.Parse(id));
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
                return -1;
            }
            myConnection.Close();

            return c;
        }

        int GetAmount_Reserved(string id)
        {
            string query = "SELECT p.Amount_Reserved FROM Product p WHERE (p.Bar_code=@Bar_code)";

            SqlConnection myConnection = GetSqlConnection();
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            SqlDataReader myreader;
            myCommand.Parameters.AddWithValue("@Bar_code", Int32.Parse(id));
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
                return -1;
            }
            myConnection.Close();

            return c;
        }

        

        public bool BuyProduct(string key, string amount)
        {
            string query = "UPDATE Product SET ";
            query += "Amount_Reserved= @Amount_Reserved WHERE Bar_code = @Bar_code";

            SqlConnection myConnection = GetSqlConnection();

            int amount_Reserved = GetAmount_Reserved(key) - Int32.Parse(amount);
            if (amount_Reserved < 0)
                return false;
            SqlCommand myCommand = new SqlCommand(query, myConnection);
            myCommand.Parameters.AddWithValue("@Bar_code", key);
            myCommand.Parameters.AddWithValue("@Amount_Reserved", amount_Reserved);
            return RunQuery(myConnection, myCommand);
        }
    }
}
