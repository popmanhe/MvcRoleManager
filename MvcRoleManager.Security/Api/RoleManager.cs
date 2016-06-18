using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcRoleManager.Security.BSO;
using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.Attributes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;

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

        [AllowAnonymous]
        [HttpGet]
        public List<ApplicationRole> GetRoles()
        {
            return RoleManagerBso.GetRoles();
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult SaveActionRoles(MvcAction mvcAction)
        {
            try
            {
                RoleManagerBso.SaveActionRoles(mvcAction);
                return Ok();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
