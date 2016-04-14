using Abp.NHibernate.EntityMappings;
using Abp.Organizations;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class OrganizationUnitMap : EntityMap<OrganizationUnit, Guid>
    {
        public OrganizationUnitMap()
            : base("AbpOrganizationUnits")
        {
            Map(x => x.TenantId);
            References(x => x.Parent).Column("ParentId").Nullable();
            //Map(x => x.ParentId);
            Map(x => x.Code);
            Map(x => x.DisplayName);

            this.MapFullAudited();
        }
    }
}