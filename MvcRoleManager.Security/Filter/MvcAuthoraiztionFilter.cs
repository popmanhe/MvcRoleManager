using System.Web.Mvc;
using System.Web;
using MvcRoleManager.Security.ViewModels;
using System.Linq;
using MvcRoleManager.Security.BSO;
using System.Security.Claims;

namespace MvcRoleManager.Security.Filter
{
    public class MvcAuthoraiztionFilter : AuthorizeAttribute
    {

        private MvcAction action;


        public MvcAuthoraiztionFilter()
        {

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var roleManagerBso = new RoleManagerBso();

            //Get id of roles that are assigned to this action
            var dbRoles = roleManagerBso.GetRolesByAction(action)?.Select(r => r.Name);
            //if no role assigned to this action, it means all roles can have access to this action
            if (dbRoles == null) return true;
            var identity = (httpContext.User.Identity as ClaimsIdentity);
            //Get the role claims that are attached to current identity
            var claimRoles = identity.Claims.Where(c => c.Type == identity.RoleClaimType).Select(r => r.Value);
            //Check if two sets of roles have something in common.
            return dbRoles.Intersect<string>(claimRoles).Count() > 0;


        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            action = new MvcAction
            {
                ActionName = filterContext.ActionDescriptor.ActionName,
                ControllerName = filterContext.Controller.ToString(),
                ParameterTypes = filterContext.ActionDescriptor.GetParameters().Select(p => p.ParameterType.ToString()),
                ReturnType = (filterContext.ActionDescriptor as ReflectedActionDescriptor).MethodInfo.ReturnParameter.ToString().Trim()
            };

            base.OnAuthorization(filterContext);
        }
    }
}
