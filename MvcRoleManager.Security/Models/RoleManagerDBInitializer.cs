using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace MvcRoleManager.Security.Models
{
    public class RoleManagerDBInitializer : DropCreateDatabaseIfModelChanges<RoleManagerDbContext>
    {
        protected override void Seed(RoleManagerDbContext context)
        {
            context.Roles.AddOrUpdate(
             p => p.Name,
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User1" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Guest" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Visitor" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Tester" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Writer" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Reader" },
             new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "No access" }
           );
        }
    }
}
