namespace ConsoleTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles");
            DropForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers");
            AddForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles");
            AddForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles", "Id");
        }
    }
}
