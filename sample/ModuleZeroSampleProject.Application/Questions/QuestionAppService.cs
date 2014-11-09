using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper;
using ModuleZeroSampleProject.Questions.Dto;

namespace ModuleZeroSampleProject.Questions
{
    public class QuestionAppService : ApplicationService, IQuestionAppService
    {
        private readonly IRepository<Question> _questionRepository;

        public QuestionAppService(IRepository<Question> questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public PagedResultOutput<QuestionDto> GetQuestions(GetQuestionsInput input)
        {
            var questionCount = _questionRepository.Count();
            var questions =
                _questionRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .OrderByDescending(q => q.CreationTime)
                    .PageBy(input)
                    .ToList();

            return new PagedResultOutput<QuestionDto>
                   {
                       TotalCount = questionCount,
                       Items = Mapper.Map<List<QuestionDto>>(questions)
                   };
        }

        [AbpAuthorize("CanCreateQuestions")]
        public void CreateQuestion(CreateQuestionInput input)
        {
            _questionRepository.Insert(new Question(input.Title, input.Text));
        }

        public GetQuestionOutput GetQuestion(GetQuestionInput input)
        {
            var question =
                _questionRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .Include(q => q.Answers)
                    .Include("Answers.CreatorUser")
                    .FirstOrDefault(q => q.Id == input.Id);

            if (question == null)
            {
                throw new UserFriendlyException("There is no such a question. Maybe it's deleted.");
            }

            if (input.IncrementViewCount)
            {
                question.ViewCount++;
            }

            return new GetQuestionOutput
                   {
                       Question = Mapper.Map<QuestionWithAnswersDto>(question)
                   };
        }
    }
}