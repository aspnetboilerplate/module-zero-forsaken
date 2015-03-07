using System.Linq;
using Abp.Domain.Repositories;
using Abp.Zero.SampleApp.Users;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Users
{
    public class UserRepository_Tests : SampleAppTestBase
    {
        [Fact]
        public void Should_Insert_And_Retrieve_User()
        {
            var useRepository = LocalIocManager.Resolve<IRepository<User, long>>();

            useRepository.FirstOrDefault(u => u.EmailAddress == "admin@aspnetboilerplate.com").ShouldBe(null);

            useRepository.Insert(new User
            {
                TenantId = null,
                UserName = "admin",
                Name = "System",
                Surname = "Administrator",
                EmailAddress = "admin@aspnetboilerplate.com",
                IsEmailConfirmed = true,
                Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
            });

            useRepository.FirstOrDefault(u => u.EmailAddress == "admin@aspnetboilerplate.com").ShouldNotBe(null);
        }
    }
}
