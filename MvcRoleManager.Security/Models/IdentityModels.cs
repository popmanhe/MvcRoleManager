using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Collections.Generic;

namespace MvcRoleManager.Security.Models
{ 
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<Action> Actions { get; set; }
    }

    public class RoleManagerDbContext : IdentityDbContext<IdentityUser>
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
                   cs.MapLeftKey("ActionId");
                   cs.MapRightKey("RoleId");
                   cs.ToTable("ActionRoles");
               });

           
        }
    }
}