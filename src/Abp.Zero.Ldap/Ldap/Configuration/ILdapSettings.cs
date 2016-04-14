using System;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;

namespace Abp.Zero.Ldap.Configuration
{
    /// <summary>
    /// Used to obtain current values of LDAP settings.
    /// This abstraction allows to define a different source for settings than SettingManager (see default implementation: <see cref="LdapSettings"/>).
    /// </summary>
    public interface ILdapSettings
    {
        Task<bool> GetIsEnabled(Guid? tenantId);

        Task<ContextType> GetContextType(Guid? tenantId);

        Task<string> GetContainer(Guid? tenantId);

        Task<string> GetDomain(Guid? tenantId);

        Task<string> GetUserName(Guid? tenantId);

        Task<string> GetPassword(Guid? tenantId);
    }
}