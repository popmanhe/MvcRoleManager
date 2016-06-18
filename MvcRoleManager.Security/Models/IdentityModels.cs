using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;

namespace MvcRoleManager.Security.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<Action> Actions { get; set; }
    }

    public class RoleManagerDbContext : IdentityDbContext<ApplicationUser>
    {
        public RoleManagerDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }
        public static RoleManagerDbContext Create()
        {
            return new RoleManagerDbContext();
        }

        static RoleManagerDbContext()
        {
#if DEBUG
            //database will only be created when tables are used.
            var config = new RoleManagerDBInitializer();

            Database.SetInitializer<RoleManagerDbContext>(config);
#else
            Database.SetInitializer<RoleManagerDbContext>(null);
#endif
        }

        public DbSet<Action> Actions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Action>()
               .HasMany<ApplicationRole>(s => s.Roles)
               .WithMany(c => c.Actions)
               .Map(cs =>
               {
                   cs.MapLeftKey("RoleId");
                   cs.MapRightKey("ActionId");
                   cs.ToTable("ActionRoles");
               });
        }
    }
}