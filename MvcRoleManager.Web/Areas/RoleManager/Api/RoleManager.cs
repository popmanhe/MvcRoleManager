﻿using System.Collections.Generic;
using System.Web.Http;
using MvcRoleManager.Web.Security.BSO;
using MvcRoleManager.Web.Security.Models;
using MvcRoleManager.Web.Security.ViewModels;
using System;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;

namespace MvcRoleManager.Web.Security.Api
{
    [RoutePrefix("api/rolemanager")]
#if DEBUG
    [AllowAnonymous]
#endif
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
                    userManagerBso = new UserManagerBso(UserManager);
                return userManagerBso;
            }
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public List<MvcController> GetControllers()
        {
            return RoleManagerBso.GetControllers();
        }

        #region Roles
        [HttpPost]
        public IHttpActionResult AddRole(MvcRole role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                RoleManagerBso.AddRole(role);
                return Ok(role.Id);
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
        public IHttpActionResult DeleteRole(MvcRole role)
        {
            RoleManagerBso.DeleteRole(role);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UpdateRole(MvcRole role)
        {
            if (ModelState.IsValid)
            {
                RoleManagerBso.UpdateRole(role);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
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
            if (ModelState.IsValid)
            {
                try
                {
                    string userId = await UserManagerBso.AddUser(user);
                    return Ok<string>(userId);
                }
                catch(Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            else
            { return BadRequest(ModelState); }
        }
        [HttpPost]
        public async Task<IHttpActionResult> DeleteUser(MvcUser user)
        {
            try
            {
                await UserManagerBso.DeleteUser(user);
                return Ok<string>(user.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public List<MvcUser> GetUsers()
        {
            return UserManagerBso.GetUsers();
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(MvcUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await UserManagerBso.UpdateUser(user);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            else
            { return BadRequest(ModelState); }
        }
        [HttpPost]
        public List<string> GetUsersByRole(MvcRole role)
        {
                return UserManagerBso.GetUsersByRole(role);
        }

        [HttpGet]
        public async Task<List<string>> GetRolesByUser(string id)
        {
           
                return await UserManagerBso.GetRolesByUser(id);
            
        }


        [HttpPost]
        public async Task<IHttpActionResult> AddRolesToUser(MvcUser user)
        {
            try
            {
                await UserManagerBso.AddRolesToUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> AddUsersToRole(MvcRole role)
        {
            try
            {
                await UserManagerBso.AddUsersToRole(role);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion
    }
}
