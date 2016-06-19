namespace MvcRoleManager.Security.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcRoleManager.Security.Models.RoleManagerDbContext>
    {
        public Configuration()
        {
#if DEBUG
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(MvcRoleManager.Security.Models.RoleManagerDbContext context)
        {
            context.Roles.AddOrUpdate(
            p => p.Name,
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Admin" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "User1" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Guest" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Visitor" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Tester" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Writer" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Reader" },
            new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "No access" }
          );
        }
    }
}
