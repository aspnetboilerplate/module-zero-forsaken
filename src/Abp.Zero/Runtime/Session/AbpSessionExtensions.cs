using System;
using Abp.Authorization.Users;

namespace Abp.Runtime.Session
{
    public static class AbpSessionExtensions
    {
        public static bool IsUser(this IAbpSession session, AbpUserBase user)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return session.TenantId == user.TenantId && 
                session.UserId.HasValue && 
                session.UserId.Value == user.Id;
        }
    }
}
