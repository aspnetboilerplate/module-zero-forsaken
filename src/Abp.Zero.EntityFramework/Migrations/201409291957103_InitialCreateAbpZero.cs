namespace Abp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateAbpZero : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpTenants",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TenancyName = c.String(),
                    Name = c.String(),
                    CreationTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AbpUsers",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TenantId = c.Int(),
                    Name = c.String(nullable: false, maxLength: 30),
                    Surname = c.String(nullable: false, maxLength: 30),
                    UserName = c.String(nullable: false, maxLength: 32),
                    Password = c.String(nullable: false, maxLength: 100),
                    EmailAddress = c.String(nullable: false, maxLength: 100),
                    IsEmailConfirmed = c.Boolean(nullable: false),
                    EmailConfirmationCode = c.String(maxLength: 16),
                    PasswordResetCode = c.String(maxLength: 32),
                    LastLoginTime = c.DateTime(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpTenants", user => user.TenantId, true, "FK_AbpUsers_TenantId_AbpTenants_Id")
                .ForeignKey("dbo.AbpUsers", user => user.CreatorUserId, false, "FK_AbpUsers_CreatorUserId_AbpUsers_Id");

            CreateTable(
                "dbo.AbpRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Name = c.String(),
                        DisplayName = c.String(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpTenants", user => user.TenantId, true, "FK_AbpRoles_TenantId_AbpTenants_Id")
                .ForeignKey("dbo.AbpUsers", role => role.CreatorUserId, false, "FK_AbpRoles_CreatorUserId_AbpUsers_Id")
                .ForeignKey("dbo.AbpUsers", role => role.LastModifierUserId, false, "FK_AbpRoles_LastModifierUserId_AbpUsers_Id");

            CreateTable(
                "dbo.AbpPermissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Int(),
                        UserId = c.Long(),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsGranted = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpRoles", permission => permission.RoleId, false, "FK_AbpPermissions_RoleId_AbpRoles_Id")
                .ForeignKey("dbo.AbpUsers", permission => permission.UserId, false, "FK_AbpPermissions_UserId_AbpUsers_Id")
                .ForeignKey("dbo.AbpUsers", permission => permission.CreatorUserId, false, "FK_AbpPermissions_CreatorUserId_AbpUsers_Id");
            
            CreateTable(
                "dbo.AbpSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        UserId = c.Long(),
                        Name = c.String(),
                        Value = c.String(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpTenants", setting => setting.TenantId, true, "FK_AbpSettings_TenantId_AbpTenants_Id")
                .ForeignKey("dbo.AbpUsers", setting => setting.UserId, false, "FK_AbpSettings_UserId_AbpUsers_Id")
                .ForeignKey("dbo.AbpUsers", setting => setting.CreatorUserId, false, "FK_AbpSettings_CreatorUserId_AbpUsers_Id")
                .ForeignKey("dbo.AbpUsers", setting => setting.LastModifierUserId, false, "FK_AbpSettings_LastModifierUserId_AbpUsers_Id");
            
            CreateTable(
                "dbo.AbpUserLogins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", userLogin => userLogin.UserId, false, "FK_AbpUserLogins_UserId_AbpUsers_Id");
            
            CreateTable(
                "dbo.AbpUserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", userRole => userRole.UserId, false, "FK_AbpUserRoles_UserId_AbpUsers_Id")
                .ForeignKey("dbo.AbpRoles", userRole => userRole.RoleId, false, "FK_AbpUserRoles_RoleId_AbpRoles_Id")
                .ForeignKey("dbo.AbpUsers", userRole => userRole.CreatorUserId, false, "FK_AbpUserRoles_CreatorUserId_AbpUsers_Id");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AbpUserRoles");
            DropTable("dbo.AbpUserLogins");
            DropTable("dbo.AbpSettings");
            DropTable("dbo.AbpPermissions");
            DropTable("dbo.AbpUsers");
            DropTable("dbo.AbpTenants");
            DropTable("dbo.AbpRoles");
        }
    }
}
