using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    public class Order_products
    {
        public string ID_order_products { get; set; }
        public string Amount { get; set; }
        public string Bar_code { get; set; }
        public string ID_client_order { get; set; }
    }
}
