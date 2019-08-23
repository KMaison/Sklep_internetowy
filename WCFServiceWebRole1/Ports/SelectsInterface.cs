using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFServiceWebRole1.Ports
{
    [ServiceContract]
    public interface SelectsInterface
    {
        [OperationContract]
        bool ifProductExist(string size, string color, string type);

        [OperationContract]
        bool ifProductAmountEnough(string id, string amount);

        [OperationContract]
        string getProductPrice(string id);

        [OperationContract]
        String[] getProductList();
    }
}
