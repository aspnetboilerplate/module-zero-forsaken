using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ModuleZeroSampleProject.Questions.Dto;

namespace ModuleZeroSampleProject.Questions
{
    public interface IQuestionAppService : IApplicationService
    {
        PagedResultOutput<QuestionDto> GetQuestions(GetQuestionsInput input);

        void CreateQuestion(CreateQuestionInput input);

        GetQuestionOutput GetQuestion(GetQuestionInput input);
    }
}
