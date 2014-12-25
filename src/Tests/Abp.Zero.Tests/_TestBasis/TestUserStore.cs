using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;

namespace Abp.Tests._TestBasis
{
    public class TestUserStore : AbpUserStore<TestTenant, TestRole, TestUser>
    {
        public TestUserStore(
            IRepository<TestUser, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<TestRole> roleRepository,
            IAbpSession session,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                userRepository,
                userLoginRepository,
                userRoleRepository,
                roleRepository,
                session,
                unitOfWorkManager)
        {

        }
    }
}