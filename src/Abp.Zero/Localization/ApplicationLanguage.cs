using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Localization
{
    /// <summary>
    /// Represents a language of the application.
    /// </summary>
    [Serializable]
    [Table("AbpLanguages")]
    public class ApplicationLanguage : FullAuditedEntity, IMayHaveTenant, IPassivable
    {
        public const int MaxNameLength = 10;
        public const int MaxDisplayNameLength = 64;
        public const int MaxIconLength = 128;

        /// <summary>
        /// TenantId of this entity. Can be null for host.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name of the culture, like "en" or "en-US".
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [StringLength(MaxIconLength)]
        public virtual string Icon { get; set; }

        /// <summary>
        /// True: This entity is active.
        /// False: This entity is not active.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Creates a new <see cref="ApplicationLanguage"/> object.
        /// </summary>
        public ApplicationLanguage()
        {
            IsActive = true;
        }

        public ApplicationLanguage(int? tenantId, string name, string displayName, string icon = null, bool isActive = true)
        {
            TenantId = tenantId;
            Name = name;
            DisplayName = displayName;
            Icon = icon;
            IsActive = isActive;
        }

        public LanguageInfo ToLanguageInfo()
        {
            return new LanguageInfo(Name, DisplayName, Icon);
        }
    }
}
