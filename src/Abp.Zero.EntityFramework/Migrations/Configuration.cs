using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace Abp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Abp.Zero.EntityFramework.AbpZeroDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Abp.Zero.EntityFramework.AbpZeroDbContext context)
        {
            //Default tenant

            context.AbpTenants.AddOrUpdate(
                t => t.TenancyName,
                new AbpTenant("Default", "Default")
                );

            //Default roles

            context.AbpRoles.AddOrUpdate(
                r => new { r.TenantId, r.Name },
                new AbpRole(null, "Admin", "Admin")
                //new AbpRole(1, "Admin", "Admin") //TODO: Not working, error: The binary operator Equal is not defined for the types 'System.Nullable`1[System.Int32]' and 'System.Int32'.
                );

            //Default users

            context.AbpUsers.AddOrUpdate(
                u => new { u.TenantId, u.UserName },
                new AbpUser
                {
                    TenantId = null,
                    UserName = "admin",
                    Name = "System",
                    Surname = "Administrator",
                    EmailAddress = "admin@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                }//, //TODO: Not working
                //new AbpUser
                //{
                //    TenantId = 1,
                //    UserName = "admin",
                //    Name = "System",
                //    Surname = "Administrator",
                //    EmailAddress = "admin@aspnetboilerplate.com",
                //    IsEmailConfirmed = true,
                //    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                //}
                );

            //User-role relations

            //context.UserRoles.AddOrUpdate(
            //    ur => new { ur.UserId, ur.RoleId },
            //    //new UserRole(1, 1)//,
            //    //new UserRole(2, 2)
            //    );
        }
    }
}
