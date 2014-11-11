using Abp.Domain.Repositories;
using Abp.Tests._TestBasis;
using Shouldly;
using Xunit;

namespace Abp.Tests.Authorization.Users
{
    public class UserRepository_Tests : TestBase
    {
        [Fact]
        public void Should_Insert_And_Retrieve_User()
        {
            var useRepository = LocalIocManager.Resolve<IRepository<TestUser, long>>();

            useRepository.FirstOrDefault(u => u.EmailAddress == "admin@aspnetboilerplate.com").ShouldBe(null);

            useRepository.Insert(new TestUser
                                 {
                                     TenantId = null,
                                     UserName = "admin",
                                     Name = "System",
                                     Surname = "Administrator",
                                     EmailAddress = "admin@aspnetboilerplate.com",
                                     IsEmailConfirmed = true,
                                     Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                                 });

            DbContext.SaveChanges();

            useRepository.FirstOrDefault(u => u.EmailAddress == "admin@aspnetboilerplate.com").ShouldNotBe(null);
        }
    }
}
