using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public abstract class AbpUser<TUser> : AbpUserBase, IFullAudited<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxUserNameLength)]
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MaxEmailAddressLength)]
        public virtual string NormalizedEmailAddress { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<UserToken> Tokens { get; set; }

        public virtual TUser DeleterUser { get; set; }

        public virtual TUser CreatorUser { get; set; }

        public virtual TUser LastModifierUser { get; set; }
        
        protected AbpUser()
        {
            Tokens = new Collection<UserToken>();
        }

        public void SetNormalizedNames()
        {
            NormalizedUserName = UserName.ToUpperInvariant();
            NormalizedEmailAddress = EmailAddress.ToUpperInvariant();
        }
    }
}