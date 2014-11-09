using Abp.Application.Services.Dto;

namespace ModuleZeroSampleProject.Questions.Dto
{
    public class AnswerDto : CreationAuditedEntityDto
    {
        public string Text { get; set; }

        public int QuestionId { get; set; }

        public bool IsAccepted { get; set; }

        public string CreatorUserName { get; set; }
    }
}