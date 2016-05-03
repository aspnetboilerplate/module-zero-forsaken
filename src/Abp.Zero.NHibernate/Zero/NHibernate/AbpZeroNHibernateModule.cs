﻿using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.NHibernate;
using System.Reflection;

namespace Abp.Zero.NHibernate
{
    /// <summary>
    /// Startup class for ABP Zero NHibernate module.
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpNHibernateModule))]
    public class AbpZeroNHibernateModule : AbpModule
    {
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