using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http;
 

namespace RoleSecurityManager.Security
{
    public class ApiAuthoraiztionFilter : AuthorizeAttribute
    {

        public ApiAuthoraiztionFilter()
        {
            
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string controllerName = actionContext.ControllerContext.Controller.ToString();
            string actionName = actionContext.ActionDescriptor.ActionName;
          
           this.Roles
               = string.Join(",",AuthorizationManager.GetRoles(controllerName, actionName));

            base.OnAuthorization(actionContext);
        }

    }
}
