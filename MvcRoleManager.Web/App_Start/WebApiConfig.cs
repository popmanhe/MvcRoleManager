using System.Web.Http;
using System.Web.Http.Dispatcher;
using MvcRoleManager.Security.Model;
using RoleSecurityManager.Security;

namespace MvcRoleManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.

            //Register web apis in external assembly
            config.Services.Replace(typeof(IAssembliesResolver), new AssembliesResolver());

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new ApiAuthoraiztionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               name: "DefaultActionApi",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
