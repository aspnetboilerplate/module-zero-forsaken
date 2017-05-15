using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
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
    }
}
