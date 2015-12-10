using System.Threading.Tasks;

namespace Abp.Organizations
{
    /// <summary>
    /// Used to get settings related to OrganizationUnits.
    /// </summary>
    public interface IOrganizationUnitSettings
    {
        /// <summary>
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for current tenant.
        /// </summary>
        int MaxUserMembershipCount { get; }

        /// <summary>
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for given tenant.
        /// </summary>
        Task<int> GetMaxUserMembershipCountAsync(int? tenantId);
    }
}