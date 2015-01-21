using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Localization;
using Abp.Tests._TestBasis;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.Identity;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Abp.Tests.Authorization.Roles
{
    public class RoleManager_Tests : TestBase
    {
        private readonly TestRoleManager _roleManager;
        
        public RoleManager_Tests()
        {
            var permissionManager = Substitute.For<IPermissionManager>();
            
            LocalIocManager.IocContainer.Register(
                Component.For<IPermissionManager>().UsingFactoryMethod(() => permissionManager).LifestyleSingleton()
                );

            _roleManager = LocalIocManager.Resolve<TestRoleManager>();
        }

        [Fact]
        public async Task Should_Create_And_Retrieve_Role()
        {
            (await _roleManager.CreateAsync(new TestRole(null, "Role1", "Role one"))).Succeeded.ShouldBe(true);
            DbContext.SaveChanges();

            var role1 = await DbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Role1");
            role1.ShouldNotBe(null);

            var role1Retrieved = await _roleManager.FindByNameAsync("Role1");

            role1Retrieved.ShouldNotBe(null);
            role1Retrieved.Name.ShouldBe("Role1");
        }
    }
}