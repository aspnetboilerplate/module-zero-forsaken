using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ModuleZeroSampleProject.Questions.Dto
{
    public class GetQuestionsInput : IInputDto, IPagedResultRequest, ISortedResultRequest, ICustomValidate
    {
        [Range(1, 1000)]
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }

        public GetQuestionsInput()
        {
            MaxResultCount = 10;
        }

        public void AddValidationErrors(List<ValidationResult> results)
        {
            var validSortingValues = new[] { "CreationTime DESC", "VoteCount DESC", "ViewCount DESC", "AnswerCount DESC" };

            if (!Sorting.IsIn(validSortingValues))
            {
                results.Add(new ValidationResult("Sorting is not valid. Valid values: " + string.Join(", ", validSortingValues)));
            }
        }
    }
}