namespace ConsoleTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Some_Collections : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AbpPermissions", "RoleId");
            CreateIndex("dbo.AbpPermissions", "UserId");
            AddForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles");
            DropForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.AbpPermissions", new[] { "UserId" });
            DropIndex("dbo.AbpPermissions", new[] { "RoleId" });
        }
    }
}
