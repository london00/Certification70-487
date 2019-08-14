using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFClientConsoleApplication_AzureBusRelay
{
    class Program
    {
        static void Main(string[] args)
        {
            string scheme = "sb";
            string serviceNamespace = "ServiceRelayWCF";
            string servicePath = "https://ServiceRelayWCF.servicebus.windows.net/mytcprelay";

            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;

            var cf = new ChannelFactory<IRechargeChannel>(
                new NetTcpRelayBinding(),
                new EndpointAddress(
                    ServiceBusEnvironment.CreateServiceUri(scheme, serviceNamespace, servicePath)
                ));

            #region Create token provider
            string policy = "RootManageSharedAccessKey";
            string accessKey = "jKp3MftOIwWfpN+mRQv3KpIdlGbuSIVbKqpF4Efdrz8=";
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(policy, accessKey);
            #endregion

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior(tokenProvider));

            using (IRechargeChannel ch = cf.CreateChannel())
            {
                Console.WriteLine(ch.GetData(1));
            }
        }
    }

    [ServiceContract(Namespace = "https://ServiceRelayWCF.servicebus.windows.net/mytcprelay")]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);
    }

    interface IRechargeChannel : IService1, IClientChannel { }
}
