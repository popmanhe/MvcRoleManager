using System.Collections.Generic;
using System.Web.Http;
using MvcRoleManager.Security.BSO;
using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.ViewModels;
using System;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace MvcRoleManager.Security.Api
{
    [RoutePrefix("api/rolemanager")]
    [AllowAnonymous]
    public class RoleManagerController : ApiController
    {
        private RoleManagerBso roleManagerBso;
        protected RoleManagerBso RoleManagerBso
        {
            get
            {
                if (roleManagerBso == null)
                    roleManagerBso = new RoleManagerBso();
                return roleManagerBso;
            }
        }

        private UserManagerBso userManagerBso;
        protected UserManagerBso UserManagerBso
        {
            get
            {
                if (userManagerBso == null)
                    userManagerBso = new UserManagerBso();
                return userManagerBso;
            }
        }


        [HttpGet]
        public List<MvcController> GetControllers()
        {
            return RoleManagerBso.GetControllers();
        }

        #region Roles
        [HttpPost]
        public ApplicationRole AddRole(ApplicationRole role)
        {
            try
            {
                RoleManagerBso.AddRole(role);
                return role;
            }
            catch (DbUpdateException dbex)
            {
                var internalEx = dbex.InnerException?.InnerException;
                if (internalEx != null)
                {
                    if (internalEx is SqlException)
                    {
                        var sqlEx = internalEx as SqlException;
                        if (sqlEx.Number == 2601)
                        {
                            throw new System.Exception(string.Format("{0} already exists. Please use another name.", role.Name));
                        }
                    }
                }
                throw new System.Exception("Add new role failed.Please try again.");
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteRole(ApplicationRole role)
        {
            RoleManagerBso.DeleteRole(role);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UpdateRole(ApplicationRole role)
        {
            RoleManagerBso.UpdateRole(role);
            return Ok();
        }
        [HttpGet]
        public List<ApplicationRole> GetRoles()
        {
            return RoleManagerBso.GetRoles();
        }

        [HttpPost]
        public List<MvcRole> GetRolesByAction(MvcAction mvcAction)
        {
            return RoleManagerBso.GetRolesByAction(mvcAction);
        }

        [HttpGet]
        public List<Models.Action> GetActionsByRole(string id)
        {
            return RoleManagerBso.GetActionsByRole(id);
        }

        [HttpPost]
        public IHttpActionResult AddActionsToRole(MvcRole role)
        {
            try
            {
                RoleManagerBso.AddActionsToRole(role);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult AddRolesToAction(MvcAction mvcAction)
        {
            try
            {
                RoleManagerBso.AddRolesToAction(mvcAction);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        #endregion

        #region Users

        [HttpPost]
        public async Task<IHttpActionResult> AddUser(MvcUser user)
        {
            try
            {
                string userId = await UserManagerBso.AddUser(user);
                return Ok<string>(userId);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public List<IdentityUser> GetUsers()
        {
            return UserManagerBso.GetUsers();
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(MvcUser user)
        {
            try
            {
                await UserManagerBso.UpdateUser(user);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
        #endregion
    }
}
