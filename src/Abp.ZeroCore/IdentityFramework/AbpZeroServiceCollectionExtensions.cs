using System;
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
        public static AbpIdentityBuilder AddAbpIdentity<TTenant, TUser, TRole>(this IServiceCollection services)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
        {
            return services.AddAbpIdentity<TTenant, TUser, TRole>(setupAction: null);
        }

        public static AbpIdentityBuilder AddAbpIdentity<TTenant, TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupAction)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
        {
            //services.TryAddScoped<UserManager<TUser>, AbpUserManager<TRole, TUser>>();
            //services.TryAddScoped(typeof(AbpUserManager<TRole, TUser>), provider => provider.GetService(typeof(UserManager<TUser>)));

            //services.TryAddScoped<RoleManager<TRole>, AbpRoleManager<TRole, TUser>>();
            //services.TryAddScoped<SignInManager<TUser>, AbpSignInManager<TTenant, TRole, TUser>>();

            return new AbpIdentityBuilder(services.AddIdentity<TUser, TRole>(setupAction), typeof(TTenant));
        }
    }
}
