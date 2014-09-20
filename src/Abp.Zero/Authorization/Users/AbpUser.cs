using System;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Abp.Utils.Helpers;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class AbpUser : CreationAuditedEntity<long>, IUser<long>, IMayHaveTenant
    {
        /// <summary>
        /// Tenant of this user.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Is the <see cref="EmailAddress"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// </summary>
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Reset code for password.
        /// It's not valid if it's null.
        /// It's for one usage and must be set to null after reset.
        /// </summary>
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// The last time this user entered to the system.
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        public virtual void GenerateEmailConfirmationCode() //TODO: Remove this method?
        {
            EmailConfirmationCode = RandomHelper.GenerateCode(16);
        }

        public virtual void GeneratePasswordResetCode() //TODO: Remove this method?
        {
            PasswordResetCode = RandomHelper.GenerateCode(32);
        }

        public virtual bool ConfirmEmail(string confirmationCode)
        {
            if (IsEmailConfirmed)
            {
                return true;
            }

            if (string.IsNullOrEmpty(EmailConfirmationCode))
            {
                throw new ApplicationException("Email confirmation code is not set for this user.");                
            }

            if (EmailConfirmationCode != confirmationCode)
            {
                return false;
            }

            IsEmailConfirmed = true;
            return true;
        }
    }
}