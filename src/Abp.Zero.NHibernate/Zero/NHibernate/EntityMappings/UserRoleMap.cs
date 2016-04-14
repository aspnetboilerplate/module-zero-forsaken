using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class UserRoleMap : EntityMap<UserRole, Guid>
    {
        public UserRoleMap()
            : base("AbpUserRoles")
        {
            Map(x => x.UserId);
            Map(x => x.RoleId);

            this.MapCreationAudited();
        }
    }
}