using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Client.Domain;
namespace WCFServiceWebRole1
{
    public class ProductsList
    {
       public List<Product> list { get; set; }
        public ProductsList()
        {
            list = new List<Product>();
        }
    }
}