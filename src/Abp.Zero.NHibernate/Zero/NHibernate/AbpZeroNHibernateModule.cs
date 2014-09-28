using System;
using System.Reflection;
using Abp.Modules;
using Abp.Startup.Infrastructure.NHibernate;

namespace Abp.Zero.NHibernate
{
    /// <summary>
    /// Startup class for ABP Zero NHibernate module.
    /// </summary>
    public class AbpZeroNHibernateModule : AbpModule
    {
        public override Type[] GetDependedModules()
        {
            return new[]
                   {
                       typeof (AbpZeroModule),
                       typeof (AbpNHibernateModule)
                   };
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpNHibernate().FluentConfiguration
                .Mappings(
                    m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
