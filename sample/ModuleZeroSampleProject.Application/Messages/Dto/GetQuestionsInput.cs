using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ModuleZeroSampleProject.Messages.Dto
{
    public class GetQuestionsInput : IInputDto, IPagedResultRequest
    {
        [Range(1, 1000)]
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public GetQuestionsInput()
        {
            MaxResultCount = 10;
        }
    }
}