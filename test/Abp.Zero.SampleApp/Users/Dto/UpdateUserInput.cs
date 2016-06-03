using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Abp.Zero.SampleApp.Users.Dto
{
    public class UpdateUserInput : IInputDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual string Surname { get; set; }

        [Required]
        public virtual string UserName { get; set; }

        [Required]
        public virtual string EmailAddress { get; set; }

        public DateTime? LastLoginTime { get; set; }
    }
}