namespace ConsoleTester.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Splitted_PermissionSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpPermissions", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpPermissions", "Discriminator");
        }
    }
}
