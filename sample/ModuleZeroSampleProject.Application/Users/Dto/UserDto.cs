using Abp.Application.Services.Dto;

namespace ModuleZeroSampleProject.Users.Dto
{
    public class UserDto : EntityDto<long>
    {
        public string UserName { get; set; }

        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string EmailAddress { get; set; }
    }
}