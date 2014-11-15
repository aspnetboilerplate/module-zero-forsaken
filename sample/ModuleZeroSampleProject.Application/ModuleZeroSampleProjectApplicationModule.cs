using System.Reflection;
using Abp.Modules;
using AutoMapper;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.Questions;
using ModuleZeroSampleProject.Questions.Dto;
using ModuleZeroSampleProject.Users;
using ModuleZeroSampleProject.Users.Dto;

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
            Mapper.CreateMap<Question, QuestionWithAnswersDto>();
            Mapper.CreateMap<Answer, AnswerDto>();
            Mapper.CreateMap<User, UserDto>();
        }
    }
}
