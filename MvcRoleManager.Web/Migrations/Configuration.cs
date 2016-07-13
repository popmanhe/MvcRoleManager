namespace MvcRoleManager.Web.Migrations
{
    using Security.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcRoleManager.Web.Security.Models.RoleManagerDbContext>
    {
        public Configuration()
        {
#if DEBUG
            AutomaticMigrationsEnabled = true;
#else
             AutomaticMigrationsEnabled = false;
#endif
        }

        protected override void Seed(MvcRoleManager.Web.Security.Models.RoleManagerDbContext context)
        {
            //  This method will be called after migrating to the latest version.
#if DEBUG
            context.Roles.AddOrUpdate(
                       p => p.Name,
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Admin" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "User1" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Guest" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Visitor" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Tester" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Writer" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Reader" },
                       new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "No access" });
#endif
        }
    }
}
