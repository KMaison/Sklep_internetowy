using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    public class Client
    {
        public string Pesel { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Order_ID { get; set; }
    }
}
