using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class UserOrganizationUnitMap : EntityMap<UserOrganizationUnit, Guid>
    {
        public UserOrganizationUnitMap()
            : base("AbpUserOrganizationUnits")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.OrganizationUnitId);

            this.MapCreationAudited();
        }
    }
}