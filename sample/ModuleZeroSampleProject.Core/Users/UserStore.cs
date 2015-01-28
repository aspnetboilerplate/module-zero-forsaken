using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.MultiTenancy;

namespace ModuleZeroSampleProject.Users
{
    public class UserStore : AbpUserStore<Tenant, Role, User>
    {
        public UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IAbpSession session)
            : base(
                userRepository,
                userLoginRepository,
                userRoleRepository,
                roleRepository,
                session)
        {
        }
    }
}