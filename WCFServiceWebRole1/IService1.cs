using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool AddProduct(string key, string size, string color, string price, string type);
        [OperationContract]
        bool UpdateProduct(string key, string size, string color, string price, string type);

        [OperationContract]
        bool AddOrderProduct(string id,string amount, string bar_code);
        [OperationContract]
        bool UpdateOrderProduct(string id, string amount, string bar_code);

        [OperationContract]
        bool AddClient(string pesel, string first_name, string surname, string order_id);
        [OperationContract]
        bool UpdateClient(string pesel, string first_name, string surname, string order_id);

        [OperationContract]
        bool AddClientOrder(string order_id, string id_order_product, string address, string order_status);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
