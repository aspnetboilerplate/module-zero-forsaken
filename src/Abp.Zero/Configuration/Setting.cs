using Abp.Domain.Entities.Auditing;

namespace Abp.Configuration
{
    /// <summary>
    /// Represents a setting.
    /// </summary>
    public class Setting : AuditedEntity<long>
    {
        /// <summary>
        /// TenantId for this setting.
        /// TenantId is null if this setting is not Tenant level.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// UserId for this setting.
        /// UserId is null if this setting is not user level.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        public virtual string Value { get; set; }

        public Setting()
        {

        }

        public Setting(int? tenantId, long? userId, string name, string value)
        {
            TenantId = tenantId;
            UserId = userId;
            Name = name;
            Value = value;
        }
    }
}