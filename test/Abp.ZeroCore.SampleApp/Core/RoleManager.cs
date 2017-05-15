using System.Collections.Generic;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abp.ZeroCore.SampleApp.Core
{
    public class RoleManager : AbpRoleManager<Role, User>
    {
        public RoleManager(
            RoleStore store, 
            IEnumerable<IRoleValidator<Role>> roleValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            ILogger<RoleManager> logger, 
            IHttpContextAccessor contextAccessor, 
            IPermissionManager permissionManager, 
            ICacheManager cacheManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            IRoleManagementConfig roleManagementConfig
            ) : base(
                store, 
                roleValidators, 
                keyNormalizer, 
                errors, 
                logger, 
                contextAccessor, 
                permissionManager, 
                cacheManager, 
                unitOfWorkManager, 
                roleManagementConfig)
        {
        }
    }
}