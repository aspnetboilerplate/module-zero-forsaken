using System.DirectoryServices.AccountManagement;

namespace Abp.Zero.Ldap.Configuration
{
    /// <summary>
    /// Used to obtain current values of LDAP settings.
    /// </summary>
    public interface ILdapConfiguration
    {
        bool IsEnabled { get; }

        ContextType ContextType { get; }

        string Container { get; }

        string Domain { get; }

        string UserName { get; }

        string Password { get; }
    }
}