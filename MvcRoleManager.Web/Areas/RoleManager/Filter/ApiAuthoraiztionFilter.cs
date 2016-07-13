using System.Web.Http.Controllers;
using System.Web.Http;
using MvcRoleManager.Web.Security.ViewModels;
using MvcRoleManager.Web.Security.BSO;
using System.Linq;
using System.Security.Claims;

namespace MvcRoleManager.Web.Security.Filter
{
    public class ApiAuthoraiztionFilter : AuthorizeAttribute
    {

        public ApiAuthoraiztionFilter()
        {

        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var roleManagerBso = new RoleManagerBso();
            MvcAction action = new MvcAction
            {
                ActionName = actionContext.ActionDescriptor.ActionName,
                ControllerName = actionContext.ControllerContext.Controller.ToString(),
                ParameterTypes = actionContext.ActionDescriptor.GetParameters().Select(p => p.ParameterType.ToString()),
                ReturnType = actionContext.ActionDescriptor.ReturnType.ToString()
            };
            //Get id of roles that are assigned to this action
            var dbRoles = roleManagerBso.GetRolesByAction(action)?.Select(r => r.Name);
            //if no role assigned to this action, it means all roles can have access to this action
            if (dbRoles == null) return true;
            var identity = (actionContext.RequestContext.Principal.Identity as ClaimsIdentity);
            //Get the role claims that are attached to current identity
            var claimRoles = identity.Claims.Where(c => c.Type == identity.RoleClaimType).Select(r => r.Value);
            //Check if two sets of roles have something in common.
            return dbRoles.Intersect<string>(claimRoles).Count() > 0;
        }
    }
}
