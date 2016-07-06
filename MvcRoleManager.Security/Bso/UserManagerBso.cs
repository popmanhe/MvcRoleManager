using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MvcRoleManager.Security.ViewModels;
using MvcRoleManager.Security.Models;
using MvcRoleManager.Security.DAL;
using Microsoft.Owin.Security;
using System.Web;
using System.Net;

namespace MvcRoleManager.Security.BSO
{
    public class UserManagerBso
    {
        private UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(RoleManagerDbContext.Create()));

        private RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(RoleManagerDbContext.Create()));

        private UnitOfWork unitOfWork = new UnitOfWork(RoleManagerDbContext.Create());
        private IAuthenticationManager Authentication
        { get; }
        public UserManagerBso(IAuthenticationManager authentication)
        {
            this.Authentication = authentication;
        }
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> AddUser(MvcUser user)
        {
            var dbUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            IdentityResult result;
            if (string.IsNullOrEmpty(user.Password))
                result = await userManager.CreateAsync(dbUser);
            else
                result = await userManager.CreateAsync(dbUser, user.Password);

            if (result.Succeeded)
            {
                dbUser = await userManager.FindByEmailAsync(user.Email);
                if (dbUser == null)
                    return null;
                return dbUser.Id;
            }

            throw new Exception("Creating user failed. " + string.Join(",", result.Errors.ToArray()));
        }

        /// <summary>
        /// Get users who belong to the role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<string> GetUsersByRole(MvcRole role)
        {
            var dbRole = this.roleManager.FindById(role.Id);
            List<string> mvcUsers = new List<string>();
            foreach (var user in dbRole.Users)
            {
                mvcUsers.Add(user.UserId);
            }
            return mvcUsers;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public List<MvcUser> GetUsers()
        {
            List<MvcUser> mvcUsers = new List<MvcUser>();
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                mvcUsers.Add(new MvcUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Selected = false
                });
            }

            return mvcUsers;
        }


        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUser(MvcUser user)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);
            dbUser.Email = user.Email;
            dbUser.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.Password))
            {//reset password instead of changing it
                string token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                await userManager.ResetPasswordAsync(user.Id, token, user.Password);
            }
            await userManager.UpdateAsync(dbUser);
        }

        /// <summary>
        /// Add users to role
        /// </summary>
        /// <param name="role">role with assigned users</param>
        /// <returns></returns>
        public async Task AddUsersToRole(MvcRole role)
        {
            string roleName = role.Name;
            var users = userManager.Users.ToList();
            //remove users from role first
            foreach (var user in users)
            {
                await this.userManager.RemoveFromRoleAsync(user.Id, roleName);
            }

            //add new users to role
            foreach (var u in role.Users)
            {
                await this.userManager.AddToRoleAsync(u.Id, roleName);
            }
        }

        public async Task<List<string>> GetRolesByUser(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            List<string> rolesId = new List<string>();
            List<MvcRole> selectedRoles = new List<MvcRole>();
            foreach (var role in user.Roles)
            {
                rolesId.Add(role.RoleId);
            }

            return rolesId;
        }

        public async Task AddRolesToUser(MvcUser user)
        {
            await ClearUserRoles(user.Id);

            if (user.Roles?.Count() > 0)
            {
                string[] rolesName = user.Roles.Select(r => r.Name).ToArray();

                await this.userManager.AddToRolesAsync(user.Id, rolesName);

            }

        }

        public async Task ClearUserRoles(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            foreach (var role in user.Roles)
            {
                this.unitOfWork.Repository<IdentityUserRole>().Delete(new IdentityUserRole { RoleId = role.RoleId, UserId = userId });
            }
            this.unitOfWork.Save();
        }

        public async Task Login(string userId)
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var user = await this.userManager.FindByIdAsync(userId);
            var claimsIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            Authentication.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);
        }
    }
}
