using System;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Threading;
using Abp.Zero.SampleApp.Authorization;
using Abp.Zero.SampleApp.Users;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Users
{
    public class UserManager_Lockout_Tests : SampleAppTestBase
    {
        private readonly UserManager _userManager;
        private readonly AppSignInManager _signInManager;
        private readonly User _testUser;

        public UserManager_Lockout_Tests()
        {
            _userManager = Resolve<UserManager>();
            _signInManager = Resolve<AppSignInManager>();

            _testUser = AsyncHelper.RunSync(() => CreateUser("TestUser"));
        }

        [Fact]
        public void Test_SupportsUserLockout()
        {
            _userManager.SupportsUserLockout.ShouldBeTrue();
        }

        [Fact]
        public async Task Test_Lockout_Full()
        {
            for (int i = 0; i < _userManager.MaxFailedAccessAttemptsBeforeLockout; i++)
            {
                (await _userManager.IsLockedOutAsync(_testUser.Id)).ShouldBeFalse();
                await _userManager.AccessFailedAsync(_testUser.Id);
            }

            (await _userManager.IsLockedOutAsync(_testUser.Id)).ShouldBeTrue();

            await Task.Delay(TimeSpan.FromSeconds(1)); //Wait for unlock
            
            (await _userManager.IsLockedOutAsync(_testUser.Id)).ShouldBeFalse();
        }

        [Fact]
        public async Task Test_Login_Lockout()
        {
            (await _signInManager.LoginAsync("TestUser", "123qwe")).Result.ShouldBe(AbpLoginResultType.Success);

            for (int i = 0; i < _userManager.MaxFailedAccessAttemptsBeforeLockout - 1; i++)
            {
                (await _signInManager.LoginAsync("TestUser", "invalid-pass")).Result.ShouldBe(AbpLoginResultType.InvalidPassword);
            }

            (await _signInManager.LoginAsync("TestUser", "invalid-pass")).Result.ShouldBe(AbpLoginResultType.LockedOut);
            (await _userManager.IsLockedOutAsync(_testUser.Id)).ShouldBeTrue();

            await Task.Delay(TimeSpan.FromSeconds(1)); //Wait for unlock

            (await _userManager.GetAccessFailedCountAsync(_testUser.Id)).ShouldBe(0);
            (await _userManager.IsLockedOutAsync(_testUser.Id)).ShouldBeFalse();
            (await _signInManager.LoginAsync("TestUser", "invalid-pass")).Result.ShouldBe(AbpLoginResultType.InvalidPassword);

            (await _signInManager.LoginAsync("TestUser", "123qwe")).Result.ShouldBe(AbpLoginResultType.Success);
            (await _userManager.GetAccessFailedCountAsync(_testUser.Id)).ShouldBe(0);
        }
    }
}