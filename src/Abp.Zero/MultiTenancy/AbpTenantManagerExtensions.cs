using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Threading;
using System;

namespace Abp.MultiTenancy
{
    //TODO: Create other sync extension methods.
    public static class AbpTenantManagerExtensions
    {
        public static TTenant GetById<TTenant, TRole, TUser>(this AbpTenantManager<TTenant, TRole, TUser> tenantManager, Guid id)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetByIdAsync(id));
        }
    }
}