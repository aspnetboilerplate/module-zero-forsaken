using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using ModuleZeroSampleProject.Users.Dto;

namespace ModuleZeroSampleProject.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;

        public UserAppService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public ListResultOutput<UserDto> GetUsers()
        {
            return new ListResultOutput<UserDto>
            {
                Items = Mapper.Map<List<UserDto>>(_userRepository.GetAllList(u => u.TenantId == CurrentSession.TenantId))
            };
        }
    }
}