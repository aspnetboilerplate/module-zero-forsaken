using Abp.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Abp.Authorization.Users;
using Abp.Authorization.Roles;

namespace Abp.IdentityFramework
{
    public static class AbpZeroIdentityBuilderExtensions
    {
        public static AbpIdentityBuilder AddAbpUserManager<TUserManager>(this AbpIdentityBuilder builder)
            where TUserManager : class
        {
            var abpManagerType = typeof(AbpUserManager<,>).MakeGenericType(builder.RoleType, builder.UserType);
            var managerType = typeof(UserManager<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(abpManagerType, services => services.GetRequiredService(managerType));
            builder.AddUserManager<TUserManager>();
            return builder;
        }

        public static AbpIdentityBuilder AddAbpRoleManager<TRoleManager>(this AbpIdentityBuilder builder)
            where TRoleManager : class
        {
            var abpManagerType = typeof(AbpRoleManager<,>).MakeGenericType(builder.RoleType, builder.UserType);
            var managerType = typeof(RoleManager<>).MakeGenericType(builder.RoleType);
            builder.Services.AddScoped(abpManagerType, services => services.GetRequiredService(managerType));
            builder.AddRoleManager<TRoleManager>();
            return builder;
        }

        public static AbpIdentityBuilder AddAbpSignInManager<TSignInManager>(this AbpIdentityBuilder builder)
            where TSignInManager : class
        {
            var abpManagerType = typeof(AbpSignInManager<,,>).MakeGenericType(builder.TenantType, builder.RoleType, builder.UserType);
            var managerType = typeof(SignInManager<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(abpManagerType, services => services.GetRequiredService(managerType));
            builder.AddSignInManager<TSignInManager>();
            return builder;
        }

        public static AbpIdentityBuilder AddAbpLogInManager<TLogInManager>(this AbpIdentityBuilder builder)
            where TLogInManager : class
        {
            var type = typeof(TLogInManager);
            var abpManagerType = typeof(AbpLogInManager<,,>).MakeGenericType(builder.TenantType, builder.RoleType, builder.UserType);
            builder.Services.AddScoped(type, provider => provider.GetService(abpManagerType));
            builder.Services.AddScoped(abpManagerType, type);
            return builder;
        }

        public static AbpIdentityBuilder AddAbpSecurityStampValidator<TSecurityStampValidator>(this AbpIdentityBuilder builder)
            where TSecurityStampValidator : class, ISecurityStampValidator
        {
            var type = typeof(TSecurityStampValidator);
            builder.Services.AddScoped(typeof(SecurityStampValidator<>).MakeGenericType(builder.UserType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(AbpSecurityStampValidator<,,>).MakeGenericType(builder.TenantType, builder.RoleType, builder.UserType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(ISecurityStampValidator), services => services.GetRequiredService(type));
            builder.Services.AddScoped(type);
            return builder;
        }

        public static AbpIdentityBuilder AddAbpUserClaimsPrincipalFactory<TUserClaimsPrincipalFactory>(this AbpIdentityBuilder builder)
            where TUserClaimsPrincipalFactory : class
        {
            var type = typeof(TUserClaimsPrincipalFactory);
            builder.Services.AddScoped(typeof(UserClaimsPrincipalFactory<,>).MakeGenericType(builder.UserType, builder.RoleType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(AbpUserClaimsPrincipalFactory<,>).MakeGenericType(builder.UserType, builder.RoleType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(builder.UserType), services => services.GetRequiredService(type));
            builder.Services.AddScoped(type);
            return builder;
        }
    }
}
