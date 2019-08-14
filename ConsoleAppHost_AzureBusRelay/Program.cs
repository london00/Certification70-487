using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFRelay_WcfServiceLibrary;

namespace ConsoleAppHost_AzureBusRelay
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Endpoint = sb://servicerelaywcf.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jKp3MftOIwWfpN+mRQv3KpIdlGbuSIVbKqpF4Efdrz8=
            string scheme = "sb";
            string serviceNamespace = "ServiceRelayWCF";
            string servicePath = "https://ServiceRelayWCF.servicebus.windows.net/mytcprelay";
            Uri address = ServiceBusEnvironment.CreateServiceUri(scheme, serviceNamespace, servicePath);

            using (ServiceHost sh = new ServiceHost(typeof(Service1), address))
            {
                var serviceEndPoint = sh.AddServiceEndpoint(typeof(IService1), new NetTcpRelayBinding(), address);

                #region Create token provider
                string policy = "RootManageSharedAccessKey";
                string accessKey = "jKp3MftOIwWfpN+mRQv3KpIdlGbuSIVbKqpF4Efdrz8=";
                var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(policy, accessKey);
                #endregion

                serviceEndPoint.Behaviors.Add(new TransportClientEndpointBehavior(tokenProvider));

                sh.Open();

                Console.WriteLine("Press ENTER to close");
                Console.ReadLine();
            }
        }
    }
}
