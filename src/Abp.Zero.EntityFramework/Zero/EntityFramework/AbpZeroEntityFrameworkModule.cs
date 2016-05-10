using Abp.Domain.Uow;
using Abp.EntityFramework;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Castle.MicroKernel.Registration;
using System.Reflection;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpEntityFrameworkModule))]
    public class AbpZeroEntityFrameworkModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IConnectionStringResolver), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IConnectionStringResolver, DbPerTenantConnectionStringResolver>()
                        .ImplementedBy<DbPerTenantConnectionStringResolver>()
                        .LifestyleTransient()
                    );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}