using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Zero.Configuration;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Users
{
    public class UserLogin_Tests : SampleAppTestBase
    {
        private readonly UserManager _userManager;

        public UserLogin_Tests()
        {
            UsingDbContext(
                context =>
                {
                    var tenant1 = context.Tenants.Add(new Tenant("tenant1", "Tenant one"));

                    context.Users.Add(
                        new User
                        {
                            Tenant = null, //Tenancy owner
                            UserName = "userOwner",
                            Name = "Owner",
                            Surname = "One",
                            EmailAddress = "owner@aspnetboilerplate.com",
                            IsEmailConfirmed = true,
                            Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                        });

                    context.Users.Add(
                        new User
                        {
                            Tenant = tenant1, //A user of tenant1
                            UserName = "user1",
                            Name = "User",
                            Surname = "One",
                            EmailAddress = "user-one@aspnetboilerplate.com",
                            IsEmailConfirmed = true,
                            Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                        });
                });

            _userManager = LocalIocManager.Resolve<UserManager>();
        }

        [Fact]
        public async Task Should_Login_With_Correct_Values_Without_MultiTenancy()
        {
            Resolve<MultiTenancyConfig>().IsEnabled = false;
            AbpSession.TenantId = 1; //TODO: We should not need to set this and implement AbpSession instead of TestSession.

            var loginResult = await _userManager.LoginAsync("user1", "123qwe");
            loginResult.Result.ShouldBe(AbpLoginResultType.Success);
            loginResult.User.Name.ShouldBe("User");
            loginResult.Identity.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Not_Login_With_Invalid_UserName_Without_MultiTenancy()
        {
            Resolve<MultiTenancyConfig>().IsEnabled = false;
            AbpSession.TenantId = 1; //TODO: We should not need to set this and implement AbpSession instead of TestSession.

            var loginResult = await _userManager.LoginAsync("wrongUserName", "asdfgh");
            loginResult.Result.ShouldBe(AbpLoginResultType.InvalidUserNameOrEmailAddress);
            loginResult.User.ShouldBe(null);
            loginResult.Identity.ShouldBe(null);
        }

        [Fact]
        public async Task Should_Login_With_Correct_Values_With_MultiTenancy()
        {
            Resolve<MultiTenancyConfig>().IsEnabled = true;

            var loginResult = await _userManager.LoginAsync("user1", "123qwe", "tenant1");
            loginResult.Result.ShouldBe(AbpLoginResultType.Success);
            loginResult.User.Name.ShouldBe("User");
            loginResult.Identity.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Login_TenancyOwner_With_Correct_Values()
        {
            Resolve<MultiTenancyConfig>().IsEnabled = true;

            var loginResult = await _userManager.LoginAsync("userOwner", "123qwe");
            loginResult.Result.ShouldBe(AbpLoginResultType.Success);
            loginResult.User.Name.ShouldBe("Owner");
            loginResult.Identity.ShouldNotBe(null);
        }
    }
}
