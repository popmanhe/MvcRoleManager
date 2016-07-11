using System.Web.Http.Controllers;
using System.Web.Http;


namespace MvcRoleManager.Security.Filter
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
          
           //this.Roles
           //    = string.Join(",",AuthorizationManager.GetRoles(controllerName, actionName));

            base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }
    }
}
