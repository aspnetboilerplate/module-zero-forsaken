namespace ConsoleTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Roles_To_User : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AbpUserRoles", "UserId");
            AddForeignKey("dbo.AbpUserRoles", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpUserRoles", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.AbpUserRoles", new[] { "UserId" });
        }
    }
}
