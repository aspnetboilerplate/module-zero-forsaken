using Abp.Domain.Entities.Auditing;

namespace ModuleZeroSampleProject.Messages
{
    public abstract class Message : CreationAuditedEntity
    {
        public virtual string Title { get; set; }

        public virtual string Text { get; set; }

        protected Message()
        {

        }

        protected Message(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}