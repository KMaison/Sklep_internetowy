using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFServiceWebRole1
{
    //TODO
    //metody typu addProduct musza byc prywatne

    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool AddProduct(string key, string size, string color, string price, string type,string amount);
        [OperationContract]
        bool UpdateProduct(string key, string size, string color, string price, string type, string amount);

        [OperationContract]
        bool AddOrderProduct(string amount, string bar_code);
        [OperationContract]
        bool UpdateOrderProduct(string id, string amount, string bar_code);

        [OperationContract]
        bool AddClient(string pesel, string first_name, string surname, string order_id);
        [OperationContract]
        bool UpdateClient(string pesel, string first_name, string surname, string order_id);

        [OperationContract]
        bool AddClientOrder(string order_id, string id_order_product, string address, string order_status);

        [OperationContract]
        bool UpdateClientOrder(string order_id, string id_order_product, string address, string order_status);

        [OperationContract]
        ProductsList GetProducts();

    }
}
