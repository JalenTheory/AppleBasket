using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AppleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppleService" in both code and config file together.
    [ServiceContract]
    public interface IAppleService
    {
        [OperationContract]
        bool AddApple(Apple apple);

        [OperationContract]
        bool ReserveApple(Apple apple);

        [OperationContract]
        bool AddSubscription(string subscription);

        [OperationContract]
        List<Apple> GetApples();
    }
}
