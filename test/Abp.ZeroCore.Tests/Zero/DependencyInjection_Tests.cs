using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.ZeroCore.SampleApp.Core;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

#if OVERRIDE_DEFAULT_SERVICES
using SecurityStampValidator = Abp.ZeroCore.SampleApp.Core.SecurityStampValidator;
#endif

namespace Abp.Zero
{
    public class DependencyInjection_Tests : AbpZeroTestBase
    {
        [Fact]
        public void Should_Resolve_UserManager()
        {
            LocalIocManager.Resolve<UserManager<User>>();
            LocalIocManager.Resolve<AbpUserManager<Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<UserManager>();
#endif
        }

        [Fact]
        public void Should_Resolve_RoleManager()
        {
            LocalIocManager.Resolve<RoleManager<Role>>();
            LocalIocManager.Resolve<AbpRoleManager<Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<RoleManager>();
#endif
        }

        [Fact]
        public void Should_Resolve_SignInManager()
        {
            LocalIocManager.Resolve<SignInManager<User>>();
            LocalIocManager.Resolve<AbpSignInManager<Tenant, Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<SignInManager>();
#endif
        }

        [Fact]
        public void Should_Resolve_LoginManager()
        {
            LocalIocManager.Resolve<AbpLogInManager<Tenant, Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<LogInManager>();
#endif
        }

        [Fact]
        public void Should_Resolve_SecurityStampValidator()
        {
            LocalIocManager.Resolve<AbpSecurityStampValidator<Tenant, Role, User>>();
            LocalIocManager.Resolve<SecurityStampValidator<User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<SecurityStampValidator>();
#endif
        }

        [Fact]
        public void Should_Resolve_UserClaimsPrincipalFactory()
        {
            LocalIocManager.Resolve<UserClaimsPrincipalFactory<User, Role>>();
            LocalIocManager.Resolve<AbpUserClaimsPrincipalFactory<User, Role>>();
            LocalIocManager.Resolve<IUserClaimsPrincipalFactory<User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<UserClaimsPrincipalFactory>();
#endif
        }

        [Fact]
        public void Should_Resolve_TenantManager()
        {
            LocalIocManager.Resolve<AbpTenantManager<Tenant, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<TenantManager>();
#endif
        }

        [Fact]
        public void Should_Resolve_EditionManager()
        {
            LocalIocManager.Resolve<AbpEditionManager>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<EditionManager>();
#endif
        }

        [Fact]
        public void Should_Resolve_PermissionChecker()
        {
            LocalIocManager.Resolve<IPermissionChecker>();
            LocalIocManager.Resolve<PermissionChecker<Tenant, Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<PermissionChecker>();
#endif
        }

        [Fact]
        public void Should_Resolve_FeatureValueStore()
        {
            LocalIocManager.Resolve<IFeatureValueStore>();
            LocalIocManager.Resolve<AbpFeatureValueStore<Tenant, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<FeatureValueStore>();
#endif
        }

        [Fact]
        public void Should_Resolve_UserStore()
        {
            LocalIocManager.Resolve<IUserStore<User>>();
            LocalIocManager.Resolve<AbpUserStore<Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<UserStore>();
#endif
        }

        [Fact]
        public void Should_Resolve_RoleStore()
        {
            LocalIocManager.Resolve<IRoleStore<Role>>();
            LocalIocManager.Resolve<AbpRoleStore<Role, User>>();

#if OVERRIDE_DEFAULT_SERVICES
            LocalIocManager.Resolve<RoleStore>();
#endif
        }
    }
}
