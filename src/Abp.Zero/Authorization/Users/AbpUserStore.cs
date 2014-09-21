using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public class AbpUserStore :
        IUserPasswordStore<AbpUser, long>,
        IUserEmailStore<AbpUser, long>,
        IUserLoginStore<AbpUser, long>,
        IUserRoleStore<AbpUser, long>,
        IQueryableUserStore<AbpUser, long>,
        ITransientDependency
    {
        #region Private fields

        private readonly IAbpUserRepository _userRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IAbpRoleRepository _roleRepository;
        private readonly IAbpSession _session;

        #endregion

        #region Constructor

        public AbpUserStore(
            IAbpUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IUserRoleRepository userRoleRepository,
            IAbpRoleRepository roleRepository,
            IAbpSession session)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;
        }

        #endregion

        #region IUserStore

        public void Dispose()
        {
            //No need to dispose since using dependency injection manager
        }

        public Task CreateAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Insert(user));
        }

        public Task UpdateAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Update(user));
        }

        public Task DeleteAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Delete(user.Id));
        }

        public Task<AbpUser> FindByIdAsync(long userId)
        {
            return Task.Factory.StartNew(() => _userRepository.FirstOrDefault(userId));
        }

        public Task<AbpUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() => _userRepository.FirstOrDefault(user => user.TenantId == _session.TenantId && (user.UserName == userName || user.EmailAddress == userName) && user.IsEmailConfirmed));
        }

        #endregion

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(AbpUser user, string passwordHash)
        {
            return Task.Factory.StartNew(() => _userRepository.UpdatePassword(user.Id, passwordHash));
        }

        public Task<string> GetPasswordHashAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Get(user.Id).Password); //TODO: Optimize
        }

        public Task<bool> HasPasswordAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => !string.IsNullOrEmpty(_userRepository.Get(user.Id).Password)); //TODO: Optimize
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(AbpUser user, string email)
        {
            return Task.Factory.StartNew(() => _userRepository.UpdateEmail(user.Id, email));
        }

        public Task<string> GetEmailAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Get(user.Id).EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(AbpUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Get(user.Id).IsEmailConfirmed); //TODO: Optimize?
        }

        public Task SetEmailConfirmedAsync(AbpUser user, bool confirmed)
        {
            return Task.Factory.StartNew(() => _userRepository.UpdateIsEmailConfirmed(user.Id, confirmed));
        }

        public Task<AbpUser> FindByEmailAsync(string email)
        {
            return Task.Factory.StartNew(() => _userRepository.FirstOrDefault(user => user.EmailAddress == email));
        }

        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(AbpUser user, UserLoginInfo login)
        {
            //TODO: Check if already exists?
            return Task.Factory.StartNew(
                () =>
                    _userLoginRepository.Insert(
                        new UserLogin
                        {
                            LoginProvider = login.LoginProvider,
                            ProviderKey = login.ProviderKey,
                            UserId = user.Id
                        })
                );
        }

        public Task RemoveLoginAsync(AbpUser user, UserLoginInfo login)
        {
            throw new NotImplementedException(); //TODO: Implement!
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AbpUser user)
        {
            return Task.Factory.StartNew<IList<UserLoginInfo>>(
                () =>
                    _userLoginRepository
                        .GetAllList(ul => ul.UserId == user.Id)
                        .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                        .ToList()
                );
        }

        public Task<AbpUser> FindAsync(UserLoginInfo login)
        {
            return Task.Factory.StartNew(
                () => FindUser(login.LoginProvider, login.ProviderKey)
                );
        }

        [UnitOfWork]
        protected virtual AbpUser FindUser(string loginProvider, string providerKey)
        {
            var query =
                from user in _userRepository.GetAll()
                join userLogin in _userLoginRepository.GetAll() on user.Id equals userLogin.UserId
                where userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey
                select user;
            return query.FirstOrDefault();
        }

        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(AbpUser user, string roleName)
        {
            //TODO: Check if already exists?

            var tenantId = _session.TenantId;

            return Task.Factory.StartNew(() =>
                                         {
                                             var role = _roleRepository.Single(r => r.Name == roleName && r.TenantId == tenantId);

                                             //TODO: Can find another way?
                                             var userRole = new UserRole
                                                            {
                                                                //User = user,
                                                                UserId = user.Id,
                                                                //Role = role,
                                                                RoleId = role.Id
                                                            };

                                             _userRoleRepository.Insert(userRole);
                                         });
        }

        public Task RemoveFromRoleAsync(AbpUser user, string roleName)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    using (var uow = new UnitOfWorkScope())
                    {
                        var query =
                            from userRole in _userRoleRepository.GetAll()
                            join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                            where userRole.UserId == user.Id && role.Name == roleName
                            select userRole;

                        var searchedUserRole = query.FirstOrDefault();

                        if (searchedUserRole == null)
                        {
                            return;
                        }

                        _userRoleRepository.Delete(searchedUserRole);

                        uow.Commit();
                    }
                });
        }

        public Task<IList<string>> GetRolesAsync(AbpUser user)
        {
            return Task.Factory.StartNew<IList<string>>(
                () =>
                {
                    using (new UnitOfWorkScope())
                    {
                        var query =
                            from userRole in _userRoleRepository.GetAll()
                            join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                            where userRole.UserId == user.Id
                            select role.Name;

                        return query.ToList();
                    }
                });
        }

        public Task<bool> IsInRoleAsync(AbpUser user, string roleName)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    using (new UnitOfWorkScope())
                    {
                        var query =
                            from userRole in _userRoleRepository.GetAll()
                            join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                            where userRole.UserId == user.Id && role.Name == roleName
                            select userRole;

                        return query.FirstOrDefault() != null;
                    }
                });
        }

        #endregion

        #region IQueryableUserStore

        public IQueryable<AbpUser> Users
        {
            get { return _userRepository.GetAll(); }
        }

        #endregion
    }
}
