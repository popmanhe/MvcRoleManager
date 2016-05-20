using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.Security.BSO;
using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.Attributes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MvcRoleManager.Security.Api
{
    [RoutePrefix("api/rolemanager")]
    public class RoleManagerController : ApiController
    {
        private RoleManagerBso _roleManagerBso;
        protected RoleManagerBso RoleManagerBso
        {
            get
            {
                this._roleManagerBso = this._roleManagerBso ?? new RoleManagerBso();
                return this._roleManagerBso;
            }
        }

        [AllowAnonymous]
        public List<MvcController> GetControllers()
        {
            return RoleManagerBso.GetControllers();
        }

        [AllowAnonymous][HttpGet]
        public List<IdentityRole> GetRoles()
        {
            return RoleManagerBso.GetRoles();
        }

        [AllowAnonymous][HttpPost]
        [DeserializeMvcAction]
        public Task<int> SaveActionRoles(List<MvcAction> actions)
        {
            //JsonSerializer serializer = new JsonSerializer();
            //serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            //serializer.MaxDepth = 10;

            //List<MvcAction> actions = JsonConvert.DeserializeObject<List<MvcAction>>(jActions["actions"].ToString());
            return RoleManagerBso.SaveActionRoles(actions);
        }

        //public Task<int> SaveActionRoles([FromBody]string actions)
        //{
        //    JsonSerializer serializer = new JsonSerializer();
        //    serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
        //    serializer.MaxDepth = 10;
        //    List<MvcAction> lstActions = JsonConvert.DeserializeObject<List<MvcAction>>(actions);
        //    return RoleManagerBso.SaveActionRoles(lstActions);
        //}
    }
}
