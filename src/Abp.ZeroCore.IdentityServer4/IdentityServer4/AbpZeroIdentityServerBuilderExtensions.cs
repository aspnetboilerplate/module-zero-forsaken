using Abp.Authorization.Users;
using Abp.IdentityServer4;
using Abp.Runtime.Security;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpZeroIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddAbpIdentityServer<TUser>(this IIdentityServerBuilder builder)
            where TUser : AbpUser<TUser>
        {
            builder.AddAspNetIdentity<TUser>();

            builder.AddProfileService<AbpProfileService<TUser>>();
            builder.AddResourceOwnerValidator<AbpResourceOwnerPasswordValidator<TUser>>();

            builder.Services.Replace(ServiceDescriptor.Transient<IClaimsService, AbpClaimsService>());

            AbpClaimTypes.UserId = JwtClaimTypes.Subject;
            AbpClaimTypes.UserName = JwtClaimTypes.Name;
            AbpClaimTypes.Role = JwtClaimTypes.Role;

            return builder;
        }
    }
}
