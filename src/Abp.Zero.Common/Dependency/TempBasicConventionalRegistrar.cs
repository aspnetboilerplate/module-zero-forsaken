using System.Reflection;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;

namespace Abp.Dependency
{
    //TODO: Remove this class and usage after upgrade to ABP v2.1

    public class TempBasicConventionalRegistrar
    {
        public void RegisterAssembly(IIocManager iocManager, Assembly assembly)
        {
            //Transient
            iocManager.IocContainer.Register(
                Classes.FromAssembly(assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<ITransientDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient()
            );

            //Singleton
            iocManager.IocContainer.Register(
                Classes.FromAssembly(assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<ISingletonDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton()
            );

            //Windsor Interceptors
            iocManager.IocContainer.Register(
                Classes.FromAssembly(assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<IInterceptor>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .LifestyleTransient()
            );
        }
    }
}