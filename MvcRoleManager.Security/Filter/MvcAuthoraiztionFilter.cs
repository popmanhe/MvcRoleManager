using System.Web.Mvc;
using System.Web;
using MvcRoleManager.Security.Model;

namespace RoleSecurityManager.Security
{
    public class MvcAuthoraiztionFilter : AuthorizeAttribute 
    {

        private MvcAction _action;
        

        public MvcAuthoraiztionFilter()
        {
            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string encrypted =  _action.GetEncryptedCode();
            this.Roles =  string.Join(",",AuthorizationManager.GetRoles(encrypted));
 
            return base.AuthorizeCore(httpContext);

 

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _action = new MvcAction();
            _action.ControllerName = filterContext.Controller.ToString();
            _action.ActionName = filterContext.ActionDescriptor.ActionName;
            //action.RouteAttribute = filterContext.RouteData.Route;
           
            base.OnAuthorization(filterContext);
        }
    }
}
