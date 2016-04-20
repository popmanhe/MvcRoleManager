using System.Web.Mvc;
using System.Web;

namespace RoleSecurityManager.Security
{
    public class MvcAuthorization: AuthorizeAttribute 
    {

        private string controllerName;
        private string actionName;

        public MvcAuthorization()
        {
            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            this.Roles =  string.Join(",",AuthorizationManager.GetRoles(this.controllerName, this.actionName));
 
            return base.AuthorizeCore(httpContext);

 

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.controllerName = filterContext.Controller.ToString();
            this.actionName = filterContext.ActionDescriptor.ActionName;

            base.OnAuthorization(filterContext);
        }
    }
}
