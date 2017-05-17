using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Abp.IdentityServer4
{
    public class AbpIdentityServerUserClaimsFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        public AbpIdentityServerUserClaimsFactory(
            UserManager<TUser> userManager,
            RoleManager<TRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor
        ) : base(userManager, roleManager, optionsAccessor)
        {
        }

        [UnitOfWork]
        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principal = await base.CreateAsync(user);

            if (user.TenantId.HasValue)
            {
                principal.Identities.First().AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }

            return principal;
        }
    }
}
