using System.Threading.Tasks;
using Abp.Zero.SampleApp.Users;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.NHibernate
{
    public class UserManager_Tests : NHibernateTestBase
    {
        private readonly UserManager _userManager;

        public UserManager_Tests()
        {
            _userManager = Resolve<UserManager>();
        }

        [Fact]
        public async Task Test1()
        {
            var admin = await _userManager.FindByNameAsync("admin");
            admin.ShouldBe(null);
        }
    }
}