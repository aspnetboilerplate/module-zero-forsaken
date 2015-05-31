using System.DirectoryServices.AccountManagement;

namespace Abp.Zero.Ldap.Configuration
{
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