using System.DirectoryServices.AccountManagement;

namespace Abp.Zero.Ldap.Authentication
{
    public class LdapConfiguration : ILdapConfiguration
    {
        public ContextType ContextType
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Container
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Domain
        {
            get { throw new System.NotImplementedException(); }
        }

        public string UserName
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Password
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}