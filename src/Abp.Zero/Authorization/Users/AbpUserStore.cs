using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public class AbpUserStore<TRole, TTenant, TUser> :
        IUserPasswordStore<TUser, long>,
        IUserEmailStore<TUser, long>,
        IUserLoginStore<TUser, long>,
        IUserRoleStore<TUser, long>,
        IQueryableUserStore<TUser, long>,
        ITransientDependency
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        #region Private fields

        private readonly IRepository<TUser, long> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<TRole> _roleRepository;
        private readonly IAbpSession _session;

        #endregion

        #region Constructor

        public AbpUserStore(
            IRepository<TUser, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<TRole> roleRepository,
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

        public Task CreateAsync(TUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Insert(user));
        }

        public Task UpdateAsync(TUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Update(user));
        }

        public Task DeleteAsync(TUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Delete(user.Id));
        }

        public Task<TUser> FindByIdAsync(long userId)
        {
            return Task.Factory.StartNew(() => _userRepository.FirstOrDefault(userId));
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() => _userRepository.FirstOrDefault(user => user.TenantId == _session.TenantId && (user.UserName == userName || user.EmailAddress == userName) && user.IsEmailConfirmed));
        }

        #endregion

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             user.Password = passwordHash;
                                             _userRepository.Update(user); //TODO: TEST
                                         });
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Get(user.Id).Password); //TODO: Optimize
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.Factory.StartNew(() => !string.IsNullOrEmpty(_userRepository.Get(user.Id).Password)); //TODO: Optimize
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(TUser user, string email)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             user.EmailAddress = email;
                                             _userRepository.Update(user); //TODO: TEST
                                         });
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Get(user.Id).EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.Factory.StartNew(() => _userRepository.Get(user.Id).IsEmailConfirmed); //TODO: Optimize?
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             user.IsEmailConfirmed = confirmed;
                                             _userRepository.Update(user); //TODO: TEST
                                         });
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            return Task.Factory.StartNew(() => _userRepository.FirstOrDefault(user => user.EmailAddress == email));
        }

        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
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

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException(); //TODO: Implement!
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return Task.Factory.StartNew<IList<UserLoginInfo>>(
                () =>
                    _userLoginRepository
                        .GetAllList(ul => ul.UserId == user.Id)
                        .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                        .ToList()
                );
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            return Task.Factory.StartNew(
                () => FindUser(login.LoginProvider, login.ProviderKey)
                );
        }

        [UnitOfWork]
        protected virtual TUser FindUser(string loginProvider, string providerKey)
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

        public Task AddToRoleAsync(TUser user, string roleName)
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

        public Task RemoveFromRoleAsync(TUser user, string roleName)
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

        public Task<IList<string>> GetRolesAsync(TUser user)
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

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
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

        public IQueryable<TUser> Users
        {
            get { return _userRepository.GetAll(); }
        }

        #endregion
    }
}
