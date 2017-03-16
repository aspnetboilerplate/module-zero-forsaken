using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public abstract class AbpUser<TUser> : AbpUserBase, IFullAudited<TUser>
        where TUser : AbpUser<TUser>
    {
        public virtual ICollection<UserToken> Tokens { get; set; }

        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }

        //TODO: Consider to remove these methods and move to tser store

        protected AbpUser()
        {
            Tokens = new Collection<UserToken>();
        }
    }
}