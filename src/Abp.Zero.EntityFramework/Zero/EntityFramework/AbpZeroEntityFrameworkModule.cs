using System;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate Zero.
    /// </summary>
    public class AbpZeroEntityFrameworkModule : AbpModule
    {
        public override Type[] GetDependedModules()
        {
            return new[]
                   {
                       typeof (AbpZeroModule),
                       typeof (AbpEntityFrameworkModule)
                   };
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
