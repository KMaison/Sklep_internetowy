using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceWebRole1.Ports
{
    [ServiceContract]
    public interface InsertsInterface
    {
        [OperationContract]
        bool addOrderProduct(string amount, string bar_code, string id_client_order);

        [OperationContract]
        bool addClient(string first_name, string surname, string order_id);
        [OperationContract]
        int createClientOrder(string address);

        [OperationContract]
        bool addClientOrder(string orderid, string address, string order_status);

        [OperationContract]
        bool addProduct(string key, string name, string size, string color, string price, string type, string amount);
        
    }
}
