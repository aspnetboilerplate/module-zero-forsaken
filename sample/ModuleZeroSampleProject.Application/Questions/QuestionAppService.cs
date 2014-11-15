using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper;
using ModuleZeroSampleProject.Questions.Dto;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Questions
{
    public class QuestionAppService : ApplicationService, IQuestionAppService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<User, long> _userRepository;

        public QuestionAppService(IRepository<Question> questionRepository, IRepository<Answer> answerRepository, IRepository<User, long> userRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _userRepository = userRepository;
        }

        public PagedResultOutput<QuestionDto> GetQuestions(GetQuestionsInput input)
        {
            var questionCount = _questionRepository.Count();
            var questions =
                _questionRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .OrderBy(input.Sorting)
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

        public VoteChangeOutput VoteUp(EntityRequestInput input)
        {
            var question = _questionRepository.Get(input.Id);
            question.VoteCount++;
            return new VoteChangeOutput(question.VoteCount);
        }

        public VoteChangeOutput VoteDown(EntityRequestInput input)
        {
            var question = _questionRepository.Get(input.Id);
            question.VoteCount--;
            return new VoteChangeOutput(question.VoteCount);
        }
        
        public SubmitAnswerOutput SubmitAnswer(SubmitAnswerInput input)
        {
            var question = _questionRepository.Get(input.QuestionId);
            var currentUser = _userRepository.Get(CurrentSession.UserId.Value);

            question.AnswerCount++;

            var answer = _answerRepository.Insert(
                new Answer(input.Text)
                {
                    Question = question,
                    CreatorUser = currentUser
                });

            return new SubmitAnswerOutput
                   {
                       Answer = Mapper.Map<AnswerDto>(answer)
                   };
        }
    }
}