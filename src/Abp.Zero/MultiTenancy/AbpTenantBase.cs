using Abp.Domain.Entities.Auditing;
using Abp.Runtime.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Base class for tenants.
    /// </summary>
    [Table("AbpTenants")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class AbpTenantBase : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// Max length of the <see cref="TenancyName"/> property.
        /// </summary>
        public const int MaxTenancyNameLength = 64;

        /// <summary>
        /// Max length of the <see cref="ConnectionString"/> property.
        /// </summary>
        public const int MaxConnectionStringLength = 1024;

        /// <summary>
        /// Tenancy name. This property is the UNIQUE name of this Tenant.
        /// It can be used as subdomain name in a web application.
        /// </summary>
        [Required]
        [StringLength(MaxTenancyNameLength)]
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// ENCRYPTED connection string of the tenant database.
        /// Can be null if this tenant is stored in host database.
        /// Use <see cref="SimpleStringCipher"/> to encrypt/decrypt this.
        /// </summary>
        [StringLength(MaxConnectionStringLength)]
        public virtual string ConnectionString { get; set; }
    }
}