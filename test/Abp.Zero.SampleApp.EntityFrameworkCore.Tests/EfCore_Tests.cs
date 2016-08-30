using System.Linq;
using System.Threading.Tasks;
using Abp.Zero.SampleApp.Roles;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.EntityFrameworkCore.Tests
{
    public class EfCore_Tests : SimpleTaskAppTestBase
    {
        private readonly RoleManager _roleManager;

        public EfCore_Tests()
        {
            _roleManager = Resolve<RoleManager>();
        }

        [Fact]
        public async Task Seed_Data_Test()
        {
            await UsingDbContextAsync(async context =>
            {
                (await context.Tenants.CountAsync()).ShouldBe(1);
            });
        }

        [Fact]
        public async Task Should_Create_And_Retrieve_Role()
        {
            await CreateRole("Role1");

            var role1Retrieved = await _roleManager.FindByNameAsync("Role1");
            role1Retrieved.ShouldNotBe(null);
            role1Retrieved.Name.ShouldBe("Role1");
        }

        [Fact]
        public async Task Multi_Tenancy_Tests()
        {
            AbpSession.TenantId = null;

            await CreateRole("HostRole1");

            UsingDbContext(context =>
            {
                context.Roles.Count().ShouldBe(1);
            });

            AbpSession.TenantId = 1;

            await CreateRole("TenantRole1");

            UsingDbContext(context =>
            {
                context.Roles.Count().ShouldBe(1);
            });
        }

        protected async Task<Role> CreateRole(string name)
        {
            return await CreateRole(name, name);
        }

        protected async Task<Role> CreateRole(string name, string displayName)
        {
            var role = new Role(AbpSession.TenantId, name, displayName);

            (await _roleManager.CreateAsync(role)).Succeeded.ShouldBe(true);

            await UsingDbContextAsync(async context =>
            {
                var createdRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == name);
                createdRole.ShouldNotBe(null);
            });

            return role;
        }
    }
}