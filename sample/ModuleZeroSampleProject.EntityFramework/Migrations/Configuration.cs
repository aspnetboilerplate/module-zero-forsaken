using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.MultiTenancy;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.EntityFramework;
using ModuleZeroSampleProject.MultiTenancy;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ModuleZeroSampleProject.EntityFramework.ModuleZeroSampleProjectDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ModuleZeroSampleProject";
        }

        protected override void Seed(ModuleZeroSampleProjectDbContext context)
        {
            CreateUserAndRoles(context);
        }

        private void CreateUserAndRoles(ModuleZeroSampleProjectDbContext context)
        {
            //Admin role for tenancy owner

            var adminRoleForTenancyOwner = context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == "Admin");
            if (adminRoleForTenancyOwner == null)
            {
                adminRoleForTenancyOwner = context.Roles.Add(new Role(null, "Admin", "Admin"));
                context.SaveChanges();
            }

            //Admin user for tenancy owner

            var adminUserForTenancyOwner = context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName == "admin");
            if (adminUserForTenancyOwner == null)
            {
                adminUserForTenancyOwner = context.Users.Add(
                    new User
                    {
                        TenantId = null,
                        UserName = "admin",
                        Name = "System",
                        Surname = "Administrator",
                        EmailAddress = "admin@aspnetboilerplate.com",
                        IsEmailConfirmed = true,
                        Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                    });

                context.SaveChanges();

                context.UserRoles.Add(new UserRole(adminUserForTenancyOwner.Id, adminRoleForTenancyOwner.Id));

                context.SaveChanges();
            }

            //Default tenant

            var defaultTenant = context.Tenants.FirstOrDefault(t => t.TenancyName == "Default");
            if (defaultTenant == null)
            {
                defaultTenant = context.Tenants.Add(new Tenant("Default", "Default"));
                context.SaveChanges();
            }

            //Admin role for 'Default' tenant

            var adminRoleForDefaultTenant = context.Roles.FirstOrDefault(r => r.TenantId == defaultTenant.Id && r.Name == "Admin");
            if (adminRoleForDefaultTenant == null)
            {
                adminRoleForDefaultTenant = context.Roles.Add(new Role(defaultTenant.Id, "Admin", "Admin"));
                context.SaveChanges();

                //Permission definitions for Admin of 'Default' tenant
                context.Permissions.Add(new RolePermissionSetting { RoleId = adminRoleForDefaultTenant.Id, Name = "CanDeleteAnswers", IsGranted = true });
                context.Permissions.Add(new RolePermissionSetting { RoleId = adminRoleForDefaultTenant.Id, Name = "CanDeleteQuestions", IsGranted = true });
                context.Permissions.Add(new RolePermissionSetting { RoleId = adminRoleForDefaultTenant.Id, Name = "CanCreateQuestions", IsGranted = true });
                context.SaveChanges();
            }

            //User role for 'Default' tenant

            var userRoleForDefaultTenant = context.Roles.FirstOrDefault(r => r.TenantId == defaultTenant.Id && r.Name == "User");
            if (userRoleForDefaultTenant == null)
            {
                userRoleForDefaultTenant = context.Roles.Add(new Role(defaultTenant.Id, "User", "User"));
                context.SaveChanges();

                //Permission definitions for User of 'Default' tenant
                context.Permissions.Add(new RolePermissionSetting { RoleId = userRoleForDefaultTenant.Id, Name = "CanCreateQuestions", IsGranted = true });
                context.SaveChanges();
            }

            //Admin for 'Default' tenant

            var adminUserForDefaultTenant = context.Users.FirstOrDefault(u => u.TenantId == defaultTenant.Id && u.UserName == "admin");
            if (adminUserForDefaultTenant == null)
            {
                adminUserForDefaultTenant = context.Users.Add(
                    new User
                    {
                        TenantId = defaultTenant.Id,
                        UserName = "admin",
                        Name = "System",
                        Surname = "Administrator",
                        EmailAddress = "admin@aspnetboilerplate.com",
                        IsEmailConfirmed = true,
                        Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                    });
                context.SaveChanges();

                context.UserRoles.Add(new UserRole(adminUserForDefaultTenant.Id, adminRoleForDefaultTenant.Id));
                context.SaveChanges();
            }

            //User 'Emre' for 'Default' tenant

            var emreUserForDefaultTenant = context.Users.FirstOrDefault(u => u.TenantId == defaultTenant.Id && u.UserName == "emre");
            if (emreUserForDefaultTenant == null)
            {
                emreUserForDefaultTenant = context.Users.Add(
                    new User
                    {
                        TenantId = defaultTenant.Id,
                        UserName = "emre",
                        Name = "Yunus Emre",
                        Surname = "Kalkan",
                        EmailAddress = "emre@aspnetboilerplate.com",
                        IsEmailConfirmed = true,
                        Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                    });
                context.SaveChanges();

                context.UserRoles.Add(new UserRole(emreUserForDefaultTenant.Id, userRoleForDefaultTenant.Id));
                context.SaveChanges();
            }
        }
    }
}
