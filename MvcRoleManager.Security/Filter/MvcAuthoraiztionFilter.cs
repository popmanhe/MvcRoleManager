using System.Web.Mvc;
using System.Web;
namespace RoleSecurityManager.Security
{
    public class MvcAuthoraiztionFilter : AuthorizeAttribute 
    {

        //private MvcAction _action;
        

        public MvcAuthoraiztionFilter()
        {
            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
         
            //this.Roles =  string.Join(",",AuthorizationManager.GetRoles(encrypted));
 
            return base.AuthorizeCore(httpContext);

 

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //_action = new MvcAction();
            //_action.ActionName = filterContext.ActionDescriptor.ActionName;
            //action.RouteAttribute = filterContext.RouteData.Route;
           
            base.OnAuthorization(filterContext);
        }
    }
}
