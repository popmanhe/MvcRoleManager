using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MvcRoleManager.Security.ViewModels;
using MvcRoleManager.Security.Models;
using System.Web.Http;

namespace MvcRoleManager.Security.BSO
{
    public class UserManagerBso
    {
        private UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(RoleManagerDbContext.Create()));

        //private RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(RoleManagerDbContext.Create()));
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

        public List<IdentityUser> GetUsers()
        {
            return userManager.Users.ToList();
        }

        public async Task UpdateUser(MvcUser user)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id);
            dbUser.Email = user.Email;
            dbUser.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.Password))
            {
                string token =   userManager.GeneratePasswordResetToken(user.Id);
                await userManager.ResetPasswordAsync(user.Id, token, user.Password);
            }
            await userManager.UpdateAsync(dbUser);
        }

        public async Task AddToRole(MvcRole role)
        {
            string roleName = role.Name;
            foreach (var u in role.Users)
            {
                await this.userManager.AddToRoleAsync(u.Id, roleName);
            }
        }
    }
}
