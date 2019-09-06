using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFServiceWebRole1
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool AddProduct(string name, string size, string color, string price, string type, string amount);
        [OperationContract]
        bool UpdateProduct(string key, string size, string color, string price, string type, string amount_Reserved, string amount_To_Reserve);

        [OperationContract]
        bool AddOrderProduct(string amount, string bar_code, string id_client_order);

        [OperationContract]
        bool AddClient(string first_name, string surname, string order_id);

        [OperationContract]
        int CreateClientOrder(string address,string mail);

        [OperationContract]
        bool AddClientOrder(string orderid, string address, string order_status,string mail);

        [OperationContract]
        bool BuyProduct(string key, string amount);

        
        
        [OperationContract]
        bool ifProductAmountEnough(string id, string amount);

        [OperationContract]
        string getProductPrice(string id);

        [OperationContract]
        String[] GetProductList();
        
        [OperationContract]
        bool ReserveProduct(string key, string amount);

        
    }
}
