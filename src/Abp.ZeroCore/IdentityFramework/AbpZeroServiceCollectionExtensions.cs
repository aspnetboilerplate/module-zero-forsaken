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
        public static IdentityBuilder AddAbpIdentity<TTenant, TUser, TRole, TUserManager, TRoleManager, TSignInManager, TSecurityStampValidator, TUserClaimsPrincipalFactory>(this IServiceCollection services)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
            where TUserManager : AbpUserManager<TRole, TUser>
            where TSignInManager : AbpSignInManager<TTenant, TRole, TUser>
            where TRoleManager : AbpRoleManager<TRole, TUser>
            where TSecurityStampValidator : AbpSecurityStampValidator<TTenant, TRole, TUser>
            where TUserClaimsPrincipalFactory: AbpUserClaimsPrincipalFactory<TUser, TRole>
        {
            return services.AddAbpIdentity<TTenant, TUser, TRole, TUserManager, TRoleManager, TSignInManager, TSecurityStampValidator, TUserClaimsPrincipalFactory>(setupAction: null);
        }

        public static IdentityBuilder AddAbpIdentity<TTenant, TUser, TRole, TUserManager, TRoleManager, TSignInManager, TSecurityStampValidator, TUserClaimsPrincipalFactory>(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
            where TUserManager : AbpUserManager<TRole, TUser>
            where TSignInManager : AbpSignInManager<TTenant, TRole, TUser>
            where TRoleManager : AbpRoleManager<TRole,TUser>
            where TSecurityStampValidator : AbpSecurityStampValidator<TTenant, TRole, TUser>
            where TUserClaimsPrincipalFactory: AbpUserClaimsPrincipalFactory<TUser, TRole>
        {
            services.TryAddScoped<UserManager<TUser>, TUserManager>();
            services.TryAddScoped<SignInManager<TUser>, TSignInManager>();
            services.TryAddScoped<RoleManager<TRole>, TRoleManager>();
            services.TryAddScoped<ISecurityStampValidator, TSecurityStampValidator>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, TUserClaimsPrincipalFactory>();

            return services.AddIdentity<TUser, TRole>(setupAction);
        }
    }
}
