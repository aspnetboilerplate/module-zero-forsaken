using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNet.Identity;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Tenant manager.
    /// Implements domain logic for <see cref="AbpTenant{TTenant,TUser}"/>.
    /// </summary>
    /// <typeparam name="TTenant">Type of the application Tenant</typeparam>
    /// <typeparam name="TRole">Type of the application Role</typeparam>
    /// <typeparam name="TUser">Type of the application User</typeparam>
    public abstract class AbpTenantManager<TTenant, TRole, TUser> : AbpServiceBase, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        private readonly IRepository<TTenant> _tenantRepository;

        protected AbpTenantManager(IRepository<TTenant> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public virtual async Task<IdentityResult> CreateAsync(TTenant tenant)
        {
            //TODO: Check duplicate TenancyName

            var validationResult = await ValidateTenantAsync(tenant);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            await _tenantRepository.InsertAsync(tenant);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TTenant tenant)
        {
            //TODO: Check duplicate TenancyName

            await _tenantRepository.UpdateAsync(tenant);
            return IdentityResult.Success;
        }

        public virtual async Task<TTenant> FindByIdAsync(int id)
        {
            return await _tenantRepository.FirstOrDefaultAsync(id);
        }

        public virtual async Task<TTenant> GetByIdAsync(int id)
        {
            var tenant = await FindByIdAsync(id);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with id: " + id);
            }

            return tenant;
        }

        public Task<TTenant> FindByTenancyNameAsync(string tenancyName)
        {
            return _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
        }

        public virtual async Task<IdentityResult> DeleteAsync(TTenant tenant)
        {
            await _tenantRepository.DeleteAsync(tenant);
            return IdentityResult.Success;
        }

        protected virtual async Task<IdentityResult> ValidateTenantAsync(TTenant tenant)
        {
            var nameValidationResult = await ValidateTenancyNameAsync(tenant.TenancyName);
            if (!nameValidationResult.Succeeded)
            {
                return nameValidationResult;
            }

            if (await _tenantRepository.CountAsync(t => t.Id != tenant.Id && t.TenancyName == tenant.TenancyName) > 0)
            {
                return new IdentityResult("There is already a tenant with given tenancy name: " + tenant.TenancyName);
            }

            return IdentityResult.Success;
        }

        protected virtual async Task<IdentityResult> ValidateTenancyNameAsync(string tenancyName)
        {
            if (!Regex.IsMatch(tenancyName, AbpTenant<TTenant, TUser>.TenancyNameRegex))
            {
                return IdentityResult.Failed("TenancyName is invalid.");
            }

            return IdentityResult.Success;
        }
    }
}