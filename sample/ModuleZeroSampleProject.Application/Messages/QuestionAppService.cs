using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper;
using ModuleZeroSampleProject.Messages.Dto;

namespace ModuleZeroSampleProject.Messages
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
    }
}