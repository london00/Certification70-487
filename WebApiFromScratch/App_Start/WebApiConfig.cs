using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using WebApiFromScratch.Models;
using WebApiFromScratch.Models.DependencyResolver;

namespace WebApiFromScratch
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(); 
            config.DependencyResolver = new UnityResolver(BuildDependencies());

            // Web API configuration and services
            config.MessageHandlers.Add(new LoggingMessageHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static UnityContainer BuildDependencies()
        {
            var container = new UnityContainer();
            container.RegisterType<IPerson, Person>();
            return container;
        }
    }
}
