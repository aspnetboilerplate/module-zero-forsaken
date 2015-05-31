using System.Threading.Tasks;
using Abp.Collections;
using Abp.Dependency;
using Abp.Modules;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Ldap
{
    public class LdapAuthenticationSource_Tests : SampleAppTestBase
    {
        private readonly UserManager _userManager;

        public LdapAuthenticationSource_Tests()
        {
            _userManager = LocalIocManager.Resolve<UserManager>();            
        }

        protected override void AddModules(ITypeList<AbpModule> modules)
        {
            base.AddModules(modules);
            modules.Add<MyUserLoginTestModule>();
        }

        //[Fact]
        public async Task Should_Login_From_Ldap()
        {
            //TODO: TEST!
        }

        [DependsOn(typeof(AbpZeroLdapModule))]
        public class MyUserLoginTestModule : AbpModule
        {
            public override void PreInitialize()
            {
                Configuration.Modules.Zero().UserManagement.ExternalAuthenticationSources.Add<MyLdapAuthenticationSource>();  //TODO: Is it possible to escape this?
            }

            public override void Initialize()
            {
                //This is needed just for this test, not for real apps
                IocManager.Register<MyLdapAuthenticationSource>(DependencyLifeStyle.Transient);
            }
        }

        public class MyLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User> //TODO: Is it possible to escape this class?
        {
            public MyLdapAuthenticationSource(ILdapConfiguration configuration)
                : base(configuration)
            {

            }
        }
    }
}
