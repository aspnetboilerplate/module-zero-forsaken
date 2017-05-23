using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Abp
{
    public class SettingsModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SettingsModule).GetAssembly());
        }
    }
}
