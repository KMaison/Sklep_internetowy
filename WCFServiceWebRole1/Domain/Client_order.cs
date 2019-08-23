using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceWebRole1.Domain
{
    public class Client_order
    {
        public string Order_ID { get; set; }
        public string Address { get; set; }
        public string Order_status { get; set; }
    }
}
