using System;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace - This is done to add extension methods to Microsoft.Extensions.DependencyInjection namespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpZeroServiceCollectionExtensions
    {
        public static IdentityBuilder AddAbpIdentity<TTenant, TUser, TRole, TSecurityStampValidator>(this IServiceCollection services)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
            where TSecurityStampValidator : AbpSecurityStampValidator<TTenant, TRole, TUser>
        {
            return services.AddAbpIdentity<TTenant, TUser, TRole, TSecurityStampValidator>(setupAction: null);
        }

        public static IdentityBuilder AddAbpIdentity<TTenant, TUser, TRole, TSecurityStampValidator>(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
            where TSecurityStampValidator : AbpSecurityStampValidator<TTenant, TRole, TUser>
        {
            services.TryAddScoped<ISecurityStampValidator, TSecurityStampValidator>();

            return services.AddIdentity<TUser, TRole>(setupAction);
        }
    }
}
