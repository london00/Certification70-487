using System;
using System.Web.Http;
using System.Web.Http.SelfHost;
using WebApiFromScratch.Models.DependencyResolver;

namespace WebAPISelfHostingConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Web API Server. Please Wait...");

            if (typeof(WebApiFromScratch.Controllers.PeopleController) == null)
            {
                // Work-around
                return;
            }

            var hostConfig = new HttpSelfHostConfiguration("http://localhost:8080");
            hostConfig.DependencyResolver = new UnityResolver(WebApiFromScratch.WebApiConfig.BuildDependencies());
            hostConfig.Routes.MapHttpRoute(
                name: "My API From Scratch",
                routeTemplate: "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
                );

            using (var server = new HttpSelfHostServer(hostConfig))
            {
                server.OpenAsync();
                // Test URL: http://localhost:8080/api/people/GetPersonByName?name=Geiser
                Console.WriteLine("Press [ENTER] to close");
                Console.ReadLine();
                server.CloseAsync();
            }
        }
    }
}
