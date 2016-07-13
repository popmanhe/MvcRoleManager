using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace MvcRoleManager.Web.Security.Models
{
    public class RoleManagerDBInitializer : DropCreateDatabaseIfModelChanges<RoleManagerDbContext>
    {
        
    }
}
