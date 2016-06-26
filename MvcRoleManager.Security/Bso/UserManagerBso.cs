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
        public string AddUser(MvcUser user)
        {
            var dbUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            IdentityResult result;
            if (string.IsNullOrEmpty(user.Passwrod))
                result= userManager.Create(dbUser);
            else
            result= userManager.Create(dbUser, user.Passwrod);

            if (result.Succeeded)
                return userManager.FindByEmail(user.Email)?.Id;

            throw new Exception("Creating user failed. " + string.Join(",", result.Errors.ToArray()));
        }

        public List<IdentityUser> GetUsers()
        {
            return userManager.Users.ToList();
        }

        public void UpdateUser(MvcUser user)
        {
            var dbUser = userManager.FindById(user.Id);
            dbUser.Email = user.Email;
            dbUser.UserName = user.UserName;
            userManager.Update(dbUser);
        }
    }
}
