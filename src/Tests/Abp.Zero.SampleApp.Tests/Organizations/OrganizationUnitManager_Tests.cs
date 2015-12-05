using System.Linq;
using System.Threading.Tasks;
using Abp.Organizations;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Organizations
{
    public class OrganizationUnitManager_Tests : SampleAppTestBase
    {
        private readonly OrganizationUnitManager _organizationUnitManager;

        public OrganizationUnitManager_Tests()
        {
            AbpSession.TenantId = GetDefaultTenant().Id;

            _organizationUnitManager = Resolve<OrganizationUnitManager>();
        }

        [Fact]
        public async Task Should_Create_Root_OU()
        {
            //Act
            await _organizationUnitManager.CreateAsync(new OrganizationUnit(AbpSession.TenantId, "Root 1"));

            //Assert
            UsingDbContext(context =>
            {
                var root1 = GetOUOrNull("Root 1");
                root1.ShouldNotBeNull();
                root1.Code.ShouldBe(OrganizationUnit.CreateUnitCode(3));
            });
        }

        [Fact]
        public async Task Should_Create_Child_OU()
        {
            //Arrange
            var ou11 = GetOU("OU11");

            //Act
            await _organizationUnitManager.CreateAsync(new OrganizationUnit(AbpSession.TenantId, "OU11 New Child", ou11.Id));

            //Assert
            UsingDbContext(context =>
            {
                var newChild = GetOUOrNull("OU11 New Child");
                newChild.ShouldNotBeNull();
                newChild.ParentId.ShouldBe(ou11.Id);
                newChild.Code.ShouldBe(OrganizationUnit.CreateUnitCode(1, 1, 3));
            });
        }

        [Fact]
        public async Task Should_Delete_UO_With_Children()
        {
            //Arrange
            var ou11 = GetOU("OU11");

            //Act
            await _organizationUnitManager.DeleteAsync(ou11.Id);

            //Assert
            UsingDbContext(context =>
            {
                GetOUOrNull("OU11").IsDeleted.ShouldBeTrue();
                GetOUOrNull("OU111").IsDeleted.ShouldBeTrue();
                GetOUOrNull("OU112").IsDeleted.ShouldBeTrue();
            });
        }

        private OrganizationUnit GetOU(string diplayName)
        {
            var organizationUnit = UsingDbContext(context => context.OrganizationUnits.FirstOrDefault(ou => ou.DisplayName == diplayName));
            organizationUnit.ShouldNotBeNull();

            return organizationUnit;
        }

        private OrganizationUnit GetOUOrNull(string diplayName)
        {
            return UsingDbContext(context => context.OrganizationUnits.FirstOrDefault(ou => ou.DisplayName == diplayName));
        }
    }
}
