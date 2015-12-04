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
        public async Task Should_Create_A_Root_OU()
        {
            await _organizationUnitManager.CreateAsync(new OrganizationUnit(AbpSession.TenantId, "Root 1"));

            UsingDbContext(context =>
            {
                var root1 = context.OrganizationUnits.FirstOrDefault(ou => ou.DisplayName == "Root 1");
                root1.ShouldNotBeNull();
                root1.Code.Length.ShouldBe(OrganizationUnit.CodeUnitLength);
            });
        }
    }
}
