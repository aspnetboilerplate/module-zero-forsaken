using Abp.Configuration.Startup;
using Abp.Dependency;
using Microsoft.AspNet.Identity;
using System;
using System.Threading;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements IAbpSession to get session informations from ASP.NET Identity framework.
    /// </summary>
    public class IdentityFrameworkClaimsAbpSession : ClaimsAbpSession, ISingletonDependency
    {
        public override Guid? UserId
        {
            get
            {
                var userIdAsString = Thread.CurrentPrincipal.Identity.GetUserId();
                if (string.IsNullOrEmpty(userIdAsString))
                {
                    return null;
                }

                Guid userId;
                if (!Guid.TryParse(userIdAsString, out userId))
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