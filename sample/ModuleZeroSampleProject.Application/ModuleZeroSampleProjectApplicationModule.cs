using System.Reflection;
using Abp.Modules;
using AutoMapper;
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

            Mapper.CreateMap<Question, QuestionDto>();
        }
    }
}
