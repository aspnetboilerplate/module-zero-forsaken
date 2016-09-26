using System;
using System.Security.Claims;
using System.Security.Principal;
using Abp.Runtime.Security;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization
{
    //TODO: Use from ABP after ABP v1.0!
    public static class ClaimsIdentityExtensions
    {
        public static int? GetTenantId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.FindFirstValue(AbpClaimTypes.TenantId);
            if (tenantIdOrNull == null)
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull);
        }
    }
}