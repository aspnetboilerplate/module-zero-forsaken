using System.Collections.Generic;

namespace ModuleZeroSampleProject.Questions.Dto
{
    public class QuestionWithAnswersDto : QuestionDto
    {
        public List<AnswerDto> Answers { get; set; }
    }
}