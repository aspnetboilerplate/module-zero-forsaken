using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class UserLoginMap : EntityMap<UserLogin, Guid>
    {
        public UserLoginMap()
            : base("AbpUserLogins")
        {
            Map(x => x.UserId);
            Map(x => x.LoginProvider);
            Map(x => x.ProviderKey);
        }
    }
}