using Microsoft.Extensions.DependencyInjection;
using Abp.IdentityFramework;

namespace Abp.ZeroCore.SampleApp.Core
{
    public static class ServicesCollectionDependencyRegistrar
    {
        public static void Register(ServiceCollection services)
        {
            services.AddLogging();

            services.AddAbpIdentity<Tenant, User, Role>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpLogInManager<LogInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpTenantManager<TenantManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddPermissionChecker<PermissionChecker>()
                .AddFeatureValueStore<FeatureValueStore>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()

                .AddDefaultTokenProviders();
        }
    }
}
