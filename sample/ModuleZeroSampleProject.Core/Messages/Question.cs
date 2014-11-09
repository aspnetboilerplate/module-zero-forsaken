using System.Collections.Generic;

namespace ModuleZeroSampleProject.Messages
{
    public class Question : Message
    {
        public virtual int VoteCount { get; set; }

        public virtual int AnswerCount { get; set; }

        public virtual int ViewCount { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public Question()
        {

        }

        public Question(string title, string text)
            : base(title, text)
        {

        }
    }
}
