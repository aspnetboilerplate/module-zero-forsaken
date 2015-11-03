using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Localization
{
    [Table("AbpLanguageTexts")]
    public class ApplicationLanguageText : AuditedEntity<long>, IMayHaveTenant
    {
        public const int MaxSourceNameLength = 128;
        public const int MaxKeyLength = 128;
        public const int MaxValueLength = 64 * 1024 * 1024; //64KB

        public int? TenantId { get; set; }

        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string LanguageName { get; set; }

        [Required]
        [StringLength(MaxSourceNameLength)]
        public virtual string Source { get; set; }

        [Required]
        [StringLength(MaxKeyLength)]
        public virtual string Key { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(MaxValueLength)]
        public virtual string Value { get; set; }
    }
}