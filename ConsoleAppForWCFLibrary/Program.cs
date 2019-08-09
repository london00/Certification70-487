using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfServiceApplication;

namespace ConsoleAppForWCFLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let the things ready...");
            var uri = new Uri(("http://localhost:8081/"));
            using (var hoster = new ServiceHost(typeof(MyFirstService), uri))
            {
                #region Add Service
                hoster.AddServiceEndpoint(typeof(IMyFirstService), new WSHttpBinding(SecurityMode.None, true), nameof(MyFirstService));
                #endregion

                #region Add metadata
                var serviceMetadataBehavior = new ServiceMetadataBehavior();
                serviceMetadataBehavior.HttpGetEnabled = true;
                hoster.Description.Behaviors.Add(serviceMetadataBehavior);
                hoster.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mexBindingHttp");
                #endregion

                hoster.Open();
                Console.WriteLine("Ready!");
                Console.ReadLine();
            }
        }
    }
}
