using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceWebRole1.Ports
{
    [ServiceContract]
    public interface UpdatesInterface
    {
         [OperationContract]
        bool updateProduct(string key, string size, string color, string price, string type, string amount_Reserved, string amount_To_Reserve);
        
        [OperationContract]
        bool updateOrderProduct(string id, string amount, string bar_code);
        
        [OperationContract]
        bool updateClient(string pesel, string first_name, string surname, string order_id);
        
        [OperationContract]
        bool updateClientOrder(string order_id, string id_order_product, string address, string order_status);
       
    }
}
