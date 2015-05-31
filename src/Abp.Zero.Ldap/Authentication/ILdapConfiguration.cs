using System.DirectoryServices.AccountManagement;

namespace Abp.Zero.Ldap.Authentication
{
    public interface ILdapConfiguration
    {
        ContextType ContextType { get; }

        string Container { get; }

        string Domain { get; }

        string UserName { get; }

        string Password { get; }
    }
}