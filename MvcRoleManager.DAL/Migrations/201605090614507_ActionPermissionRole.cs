namespace MvcRoleManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActionPermissionRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionRolePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControllerName = c.String(nullable: false, maxLength: 256),
                        ActionName = c.String(nullable: false, maxLength: 256),
                        ParameterTypes = c.String(),
                        ReturnType = c.String(),
                        Roles = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ControllerName, t.ActionName }, name: "IX_RolePermission_CtrlAct");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActionRolePermissions", "IX_RolePermission_CtrlAct");
            DropTable("dbo.ActionRolePermissions");
        }
    }
}
