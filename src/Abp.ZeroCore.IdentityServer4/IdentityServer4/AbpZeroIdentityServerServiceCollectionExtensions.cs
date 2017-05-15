using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.IdentityServer4;
using Abp.MultiTenancy;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpZeroIdentityServerServiceCollectionExtensions
    {
        public static IIdentityServerBuilder AddAbpIdentityServer<TTenant, TUser,TRole>(this IIdentityServerBuilder builder)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
        {
            builder.AddProfileService<AbpProfileService<TRole, TUser>>();

            return builder;
        }
    }
}
