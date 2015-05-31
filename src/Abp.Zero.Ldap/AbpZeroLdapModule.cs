using System.Reflection;
using Abp.Modules;

namespace Abp.Zero.Ldap
{
    [DependsOn(typeof (AbpZeroCoreModule))]
    public class AbpZeroLdapModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
