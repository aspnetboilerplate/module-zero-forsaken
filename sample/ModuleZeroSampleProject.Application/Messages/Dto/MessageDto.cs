using System;
using Abp.Application.Services.Dto;

namespace ModuleZeroSampleProject.Messages.Dto
{
    public class MessageDto : IDto
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public long CreatorUserId { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime CreationTime { get; set; }
    }
}