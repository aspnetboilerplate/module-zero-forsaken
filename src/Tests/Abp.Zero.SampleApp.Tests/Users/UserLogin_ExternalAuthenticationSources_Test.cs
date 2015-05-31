using Abp.Zero.SampleApp.Users;

namespace Abp.Zero.SampleApp.Tests.Users
{
    public class UserLogin_ExternalAuthenticationSources_Test : SampleAppTestBase
    {
        private readonly UserManager _userManager;

        public UserLogin_ExternalAuthenticationSources_Test()
        {
            UsingDbContext(UserLoginHelper.CreateTestUsers);
            _userManager = LocalIocManager.Resolve<UserManager>();
        }

        public void Should_Login_From_Fake_Authentication_Source()
        {
            
        }

        public void Should_Fallback_To_Default_Login_Users()
        {

        }
    }
}