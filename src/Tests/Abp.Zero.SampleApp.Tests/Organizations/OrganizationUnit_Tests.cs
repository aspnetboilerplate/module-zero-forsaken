using Abp.Organizations;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Organizations
{
    public class OrganizationUnit_Tests
    {
        [Fact]
        public void Test_CreateUnitCode()
        {
            OrganizationUnit.CreateUnitCode(1, 1, 3).ShouldBe("00001.00001.00003");
        }
    }
}