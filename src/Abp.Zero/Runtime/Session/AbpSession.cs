using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Abp.Dependency;
using Abp.Runtime.Security;
using Microsoft.AspNet.Identity;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements IAbpSession to get session informations from ASP.NET Identity framework.
    /// </summary>
    public class AbpSession : IAbpSession, ISingletonDependency
    {
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
                if (!IsMultiTenancyEnabled)
                {
                    return 1; //TODO: !!!
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

        private bool IsMultiTenancyEnabled
        {
            get { return string.Equals(ConfigurationManager.AppSettings["Abp.MultiTenancy.IsEnabled"], "true", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}