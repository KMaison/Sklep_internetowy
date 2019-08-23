using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using WCFServiceWebRole1.Ports;

namespace WCFServiceWebRole1.Adapters
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SelectsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SelectsService.svc or SelectsService.svc.cs at the Solution Explorer and start debugging.
    public class SelectsService : SelectsInterface
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

        public string[] getProductList()
        {
            string query = "SELECT * FROM Product";
            SqlDataReader myreader;

            SqlConnection myConnection = getSqlConnection();
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
                        myreader[6].ToString() + ";" +
                        myreader[7].ToString();
                    productList[i] = tmp;
                    i++;

                }
            }
            myConnection.Close();
            return productList;
        }

        private int countProduct()
        {
            string query = "SELECT COUNT(p.Name) FROM Product p  ";

            SqlConnection myConnection = getSqlConnection();
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

        public string getProductPrice(string id)
        {
            string query = "SELECT p.Price FROM Product p WHERE (p.Bar_code=@Bar_code) ";

            SqlConnection myConnection = getSqlConnection();
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

        public bool ifProductAmountEnough(string id, string amount)
        {
            string query = "SELECT p.Name FROM Product p WHERE (p.Bar_code=@Bar_code AND p.Amount_To_Reserve>=@Amount) ";

            SqlConnection myConnection = getSqlConnection();
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

        public bool ifProductExist(string size, string color, string type)
        {
            string query = "SELECT p.Name FROM Product p WHERE (p.Size=@Size AND p.Color=@Color AND p.Clothes_type=@Type) ";

            SqlConnection myConnection = getSqlConnection();
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
    }
}
