using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to store a User Login for external Login services.
    /// </summary>
    [Table("AbpUserLogins")]
    public class UserLogin : Entity<long>
    {
        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Login Provider.
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key in the <see cref="LoginProvider"/>.
        /// </summary>
        public virtual string ProviderKey { get; set; }
    }
}
