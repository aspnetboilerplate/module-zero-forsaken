namespace ModuleZeroSampleProject.Messages.Dto
{
    public class AnswerDto : MessageDto
    {
        public int QuestionId { get; set; }

        public bool IsAccepted { get; set; }
    }
}