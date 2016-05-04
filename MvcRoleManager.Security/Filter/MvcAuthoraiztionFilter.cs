using System.Web.Mvc;
using System.Web;
using MvcRoleManager.Security.Model;

namespace RoleSecurityManager.Security
{
    public class MvcAuthoraiztionFilter : AuthorizeAttribute 
    {

        private string encryptedAction;
        

        public MvcAuthoraiztionFilter()
        {
            
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            this.Roles =  string.Join(",",AuthorizationManager.GetRoles(encryptedAction));
 
            return base.AuthorizeCore(httpContext);

 

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var action = new MvcAction();
            action.ControllerName = filterContext.Controller.ToString();
            action.ActionName = filterContext.ActionDescriptor.ActionName;
            action.RouteAttribute = filterContext.RouteData.Route;
            encryptedAction = Util.EncryptAction(action);

            base.OnAuthorization(filterContext);
        }
    }
}
