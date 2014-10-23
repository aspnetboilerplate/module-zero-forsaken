namespace ConsoleTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Logins_To_User : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AbpUserLogins", "UserId");
            AddForeignKey("dbo.AbpUserLogins", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpUserLogins", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.AbpUserLogins", new[] { "UserId" });
        }
    }
}
