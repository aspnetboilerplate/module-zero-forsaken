using System.Threading;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Microsoft.AspNet.Identity;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements IAbpSession to get session informations from ASP.NET Identity framework.
    /// </summary>
    public class IdentityFrameworkClaimsAbpSession : ClaimsAbpSession, ISingletonDependency
    {
        public override long? UserId
        {
            get
            {
                var userIdAsString = Thread.CurrentPrincipal.Identity.GetUserId();
                if (string.IsNullOrEmpty(userIdAsString))
                {
                    return null;
                }

                long userId;
                if (!long.TryParse(userIdAsString, out userId))
                {
                    return null;
                }

                return userId;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IdentityFrameworkClaimsAbpSession(IMultiTenancyConfig multiTenancy) 
            : base(multiTenancy)
        {
        }
    }
}