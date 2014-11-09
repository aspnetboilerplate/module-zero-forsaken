using System.Reflection;
using Abp.Modules;
using AutoMapper;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.Messages;
using ModuleZeroSampleProject.Messages.Dto;

namespace ModuleZeroSampleProject
{
    [DependsOn(typeof(ModuleZeroSampleProjectCoreModule))]
    public class ModuleZeroSampleProjectApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Authorization.Providers.Add<ModuleZeroSampleProjectAuthorizationProvider>();

            Mapper.CreateMap<Question, QuestionDto>();
        }
    }
}
