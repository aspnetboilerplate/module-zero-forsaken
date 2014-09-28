using Abp.Authorization.Roles;
using Abp.Domain.Entities.Mapping;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class RoleMapBase<TRole> : EntityMap<TRole> where TRole : AbpRole
    {
        protected RoleMapBase()
            : base("AbpRoles")
        {
            Map(x => x.TenantId);
            Map(x => x.Name);
            Map(x => x.DisplayName);
            
            this.MapAudited();

            Polymorphism.Explicit();
        }
    }
}