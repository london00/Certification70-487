﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFRelay_WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "https://ServiceRelayWCF.servicebus.windows.net/mytcprelay")]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);
    }
}
