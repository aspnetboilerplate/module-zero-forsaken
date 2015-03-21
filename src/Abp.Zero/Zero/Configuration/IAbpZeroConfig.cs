using System.Security.Policy;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// Configuration options for zero module.
    /// </summary>
    public interface IAbpZeroConfig
    {
        /// <summary>
        /// Gets user management config.
        /// </summary>
        IUserManagementConfig UserManagement { get; }
    }
}