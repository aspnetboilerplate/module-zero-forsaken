using Abp.Modules;
using Abp.TestBase;
using Abp.Zero.Ldap;
using Abp.Zero.SampleApp.EntityFramework;

namespace Abp.Zero.SampleApp.Tests
{
    [DependsOn(
        typeof(SampleAppEntityFrameworkModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpTestBaseModule))]
    public class SampleAppTestModule : AbpModule
    {

    }
}