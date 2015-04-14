using Abp.IdentityFramework;
using Abp.Localization;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.IdentityFramework
{
    public class IdentityResultHelper_Tests : SampleAppTestBase
    {
        [Fact]
        public void Should_Localize_IdentityFramework_Messages()
        {
            var localizationManager = Resolve<ILocalizationManager>();

            IdentityResultHelper
                .LocalizeErrorMessage("Incorrect password.", localizationManager)
                .ShouldBe("Incorrect password.");

            IdentityResultHelper
                .LocalizeErrorMessage("Passwords must be at least 6 characters.", localizationManager)
                .ShouldBe("Passwords must be at least 6 characters.");
        }
    }
}
