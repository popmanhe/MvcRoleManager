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
        private RoleManagerDbContext roleManagerDbContext;
        private UserManager<IdentityUser> UserManager { get; set; }
        private RoleManager<ApplicationRole> RoleManager { get; set; }
        private UnitOfWork UnitOfWork { get; set; }

        public UserManagerBso()
        {
            this.roleManagerDbContext = RoleManagerDbContext.Create();
            this.UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(this.roleManagerDbContext));
            this.RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(this.roleManagerDbContext));
            this.UnitOfWork = new UnitOfWork(this.roleManagerDbContext);
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
                result = await UserManager.CreateAsync(dbUser);
            else
                result = await UserManager.CreateAsync(dbUser, user.Password);

            if (result.Succeeded)
            {
                dbUser = await UserManager.FindByEmailAsync(user.Email);
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

            var dbRole = this.RoleManager.FindById(role.Id);
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
            var users = UserManager.Users.ToList();
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
            var dbUser = await UserManager.FindByIdAsync(user.Id);
            dbUser.Email = user.Email;
            dbUser.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.Password))
            {//reset password instead of changing it
                string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                await UserManager.ResetPasswordAsync(user.Id, token, user.Password);
            }
            await UserManager.UpdateAsync(dbUser);
        }

        /// <summary>
        /// Add users to role
        /// </summary>
        /// <param name="role">role with assigned users</param>
        /// <returns></returns>
        public async Task AddUsersToRole(MvcRole role)
        {
            string roleName = role.Name;
            var users = UserManager.Users.ToList();
            //remove users from role first
            foreach (var user in users)
            {
                await this.UserManager.RemoveFromRoleAsync(user.Id, roleName);
            }

            //add new users to role
            foreach (var u in role.Users)
            {
                await this.UserManager.AddToRoleAsync(u.Id, roleName);
            }
        }

        public async Task<List<string>> GetRolesByUser(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            List<string> rolesId = new List<string>();
            foreach (var role in user.Roles)
            {
                rolesId.Add(role.RoleId);
            }

            return rolesId;
        }

        public async Task DeleteUser(MvcUser user)
        {
            var dbUser = await this.UserManager.FindByIdAsync(user.Id);
            await this.UserManager.DeleteAsync(dbUser);
        }

        public async Task AddRolesToUser(MvcUser user)
        {
            await ClearUserRoles(user.Id);

            if (user.Roles?.Count() > 0)
            {
                string[] rolesName = user.Roles.Select(r => r.Name).ToArray();

                await this.UserManager.AddToRolesAsync(user.Id, rolesName);
            }
        }
        public async Task ClearUserRoles(string userId)
        {
            var user = await this.UserManager.FindByIdAsync(userId);
            foreach (var role in user.Roles)
            {
                this.UnitOfWork.Repository<IdentityUserRole>().Delete(new IdentityUserRole { RoleId = role.RoleId, UserId = userId });
            }
            this.UnitOfWork.Save();
        }
    }
}
