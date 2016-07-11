using System.Web.Http;
using System.Web.Http.Dispatcher;
using MvcRoleManager.Security;
using MvcRoleManager.Security.Filter;
using Microsoft.Owin.Security.OAuth;

namespace MvcRoleManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.

            //Register web apis from external assemblies
            config.Services.Replace(typeof(IAssembliesResolver), new AssembliesResolver());

            config.SuppressDefaultHostAuthentication();
            //This filter would generate identity claim from bearer token
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new ApiAuthoraiztionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
               name: "DefaultActionApi",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

        }
    }
}
