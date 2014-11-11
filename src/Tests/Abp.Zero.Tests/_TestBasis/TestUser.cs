using Abp.Authorization.Users;

namespace Abp.Tests._TestBasis
{
    public class TestUser : AbpUser<TestTenant, TestUser>
    {
        public override string ToString()
        {
            return string.Format("[TestUser {0}] {1}", Id, UserName);
        }
    }
}