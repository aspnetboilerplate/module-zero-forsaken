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
using SecurityStampValidator = Abp.ZeroCore.SampleApp.Core.SecurityStampValidator;

namespace Abp.Zero
{
    public class DependencyInjection_Tests : AbpZeroTestBase
    {
        [Fact]
        public void Should_Resolve_UserManager()
        {
            var manager = LocalIocManager.Resolve<UserManager<User>>();
            LocalIocManager.Resolve<UserManager>().ShouldBeOfType(manager.GetType());
            LocalIocManager.Resolve<AbpUserManager<Role, User>>().ShouldBeOfType(manager.GetType());
        }
        
        [Fact]
        public void Should_Resolve_RoleManager()
        {
            var manager = LocalIocManager.Resolve<RoleManager<Role>>();
            LocalIocManager.Resolve<RoleManager>().ShouldBeOfType(manager.GetType());
            LocalIocManager.Resolve<AbpRoleManager<Role, User>>().ShouldBeOfType(manager.GetType());
        }

        [Fact]
        public void Should_Resolve_SignInManager()
        {
            var manager = LocalIocManager.Resolve<SignInManager<User>>();
            LocalIocManager.Resolve<SignInManager>().ShouldBeOfType(manager.GetType());
            LocalIocManager.Resolve<AbpSignInManager<Tenant, Role, User>>().ShouldBeOfType(manager.GetType());
        }

        [Fact]
        public void Should_Resolve_LoginManager()
        {
            var manager = LocalIocManager.Resolve<AbpLogInManager<Tenant, Role, User>>();
            LocalIocManager.Resolve<LogInManager>().ShouldBeOfType(manager.GetType());
        }

        [Fact]
        public void Should_Resolve_SecurityStampValidator()
        {
            var validator = LocalIocManager.Resolve<AbpSecurityStampValidator<Tenant, Role, User>>();
            LocalIocManager.Resolve<SecurityStampValidator>().ShouldBeOfType(validator.GetType());
            LocalIocManager.Resolve<SecurityStampValidator<User>>().ShouldBeOfType(validator.GetType());
        }

        [Fact]
        public void Should_Resolve_UserClaimsPrincipalFactory()
        {
            var factory = LocalIocManager.Resolve<UserClaimsPrincipalFactory>();
            LocalIocManager.Resolve<UserClaimsPrincipalFactory<User, Role>>().ShouldBeOfType(factory.GetType());
            LocalIocManager.Resolve<AbpUserClaimsPrincipalFactory<User, Role>>().ShouldBeOfType(factory.GetType());
            LocalIocManager.Resolve<IUserClaimsPrincipalFactory<User>>().ShouldBeOfType(factory.GetType());
        }

        [Fact]
        public void Should_Resolve_TenantManager()
        {
            var manager = LocalIocManager.Resolve<TenantManager>();
            LocalIocManager.Resolve<AbpTenantManager<Tenant, User>>().ShouldBeOfType(manager.GetType());
        }

        [Fact]
        public void Should_Resolve_EditionManager()
        {
            var manager = LocalIocManager.Resolve<EditionManager>();
            LocalIocManager.Resolve<AbpEditionManager>().ShouldBeOfType(manager.GetType());
        }

        [Fact]
        public void Should_Resolve_PermissionChecker()
        {
            var checker = LocalIocManager.Resolve<PermissionChecker>();
            LocalIocManager.Resolve<IPermissionChecker>().ShouldBeOfType(checker.GetType());
            LocalIocManager.Resolve<PermissionChecker<Tenant, Role, User>>().ShouldBeOfType(checker.GetType());
        }

        [Fact]
        public void Should_Resolve_FeatureValueStore()
        {
            var checker = LocalIocManager.Resolve<FeatureValueStore>();
            LocalIocManager.Resolve<IFeatureValueStore>().ShouldBeOfType(checker.GetType());
            LocalIocManager.Resolve<AbpFeatureValueStore<Tenant, User>>().ShouldBeOfType(checker.GetType());
        }

        [Fact]
        public void Should_Resolve_UserStore()
        {
            var store = LocalIocManager.Resolve<UserStore>();
            LocalIocManager.Resolve<IUserStore<User>>().ShouldBeOfType(store.GetType());
            LocalIocManager.Resolve<AbpUserStore<Role, User>>().ShouldBeOfType(store.GetType());
        }

        [Fact]
        public void Should_Resolve_RoleStore()
        {
            var store = LocalIocManager.Resolve<RoleStore>();
            LocalIocManager.Resolve<IRoleStore<Role>>().ShouldBeOfType(store.GetType());
            LocalIocManager.Resolve<AbpRoleStore<Role, User>>().ShouldBeOfType(store.GetType());
        }
    }
}
