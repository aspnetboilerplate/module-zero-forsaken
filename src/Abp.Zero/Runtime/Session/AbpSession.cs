using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Abp.Dependency;
using Abp.Runtime.Security;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements IAbpSession to get session informations from ASP.NET Identity framework.
    /// </summary>
    public class AbpSession : IAbpSession, ISingletonDependency
    {
        private readonly MultiTenancyConfig _multiTenancy;

        public long? UserId
        {
            get
            {
                var userId = Thread.CurrentPrincipal.Identity.GetUserId();
                if (userId == null)
                {
                    return null;
                }

                return Convert.ToInt64(userId);
            }
        }

        public int? TenantId
        {
            get
            {
                if (!_multiTenancy.IsEnabled)
                {
                    return 1; //TODO: This assumption is not good!
                }

                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return null;
                }

                var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
                if (claim == null)
                {
                    return null;
                }

                return Convert.ToInt32(claim.Value);
            }
        }

        public AbpSession(MultiTenancyConfig multiTenancy)
        {
            _multiTenancy = multiTenancy;
        }
    }
}