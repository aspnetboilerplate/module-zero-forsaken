namespace ModuleZeroSampleProject.Messages.Dto
{
    public class QuestionDto : MessageDto
    {
        public int VoteCount { get; set; }

        public int AnswerCount { get; set; }

        public int ViewCount { get; set; }
    }
}