using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ModuleZeroSampleProject.Messages.Dto;

namespace ModuleZeroSampleProject.Messages
{
    public interface IQuestionAppService : IApplicationService
    {
        PagedResultOutput<QuestionDto> GetQuestions(GetQuestionsInput input);
    }
}
