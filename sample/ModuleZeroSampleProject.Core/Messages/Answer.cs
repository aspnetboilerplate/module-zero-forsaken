namespace ModuleZeroSampleProject.Messages
{
    public class Answer : Message
    {
        public virtual Question Question { get; set; }

        public virtual int QuestionId { get; set; }

        public virtual bool IsAccepted { get; set; }

        public Answer()
        {
            
        }

        public Answer(string title, string text)
            : base(title, text)
        {
            
        }
    }
}