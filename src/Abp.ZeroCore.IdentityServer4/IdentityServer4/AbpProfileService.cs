using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Runtime.Security;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Abp.IdentityServer4
{
    public class AbpProfileService<TRole, TUser> : ProfileService<TUser> 
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        protected AbpProfileService( //TODO: Check if correct arguments passed
            UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IUnitOfWorkManager unitOfWorkManager
        ) : base(userManager, claimsFactory)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var tenantId = context.Subject.Identity.GetTenantId();
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await base.GetProfileDataAsync(context);
            }
        }

        [UnitOfWork]
        public override async Task IsActiveAsync(IsActiveContext context)
        {
            var tenantId = context.Subject.Identity.GetTenantId();
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await base.IsActiveAsync(context);
            }
        }
    }
}
