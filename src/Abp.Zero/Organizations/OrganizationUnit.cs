using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Organizations
{
    /// <summary>
    /// Represents an organization unit (OU).
    /// </summary>
    [Table("AbpOrganizationUnits")]
    public class OrganizationUnit : FullAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="DisplayName"/> property.
        /// </summary>
        public const int MaxDisplayNameLength = 128;

        /// <summary>
        /// Maximum depth of an UO hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// Maximum length of the <see cref="Code"/> property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;

        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Parent <see cref="OrganizationUnit"/>.
        /// Null, if this OU is root.
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual OrganizationUnit Parent { get; set; }

        /// <summary>
        /// Parent <see cref="OrganizationUnit"/> Id.
        /// Null, if this OU is root.
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// Hierarchical Code of this organization unit.
        /// Example: "00001.00042.00005".
        /// This is a unique code for a Tenant.
        /// It's changeable if OU hierarch is changed.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string Code { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Children of this OU.
        /// </summary>
        public virtual ICollection<OrganizationUnit> Children { get; set; }

        public OrganizationUnit()
        {
            
        }

        public OrganizationUnit(int? tenantId, string displayName, long? parentId = null)
        {
            TenantId = tenantId;
            DisplayName = displayName;
            ParentId = parentId;
        }
    }
}
