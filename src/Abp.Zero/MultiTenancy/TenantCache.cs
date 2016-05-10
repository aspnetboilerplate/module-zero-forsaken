using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using System;

namespace Abp.MultiTenancy
{
    public class TenantCache<TTenant, TUser> : ITenantCache, IEventHandler<EntityChangedEventData<TTenant>>
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TenantCache(
            ICacheManager cacheManager,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual TenantCacheItem Get(Guid tenantId)
        {
            return _cacheManager
                .GetTenantCache()
                .Get(
                    tenantId,
                    () =>
                    {
                        var tenant = GetTenant(tenantId);
                        return CreateTenantCacheItem(tenant);
                    }
                );
        }

        protected virtual TenantCacheItem CreateTenantCacheItem(TTenant tenant)
        {
            return new TenantCacheItem
            {
                Name = tenant.Name,
                TenancyName = tenant.TenancyName,
                EditionId = tenant.EditionId,
                ConnectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString),
                IsActive = tenant.IsActive
            };
        }

        [UnitOfWork]
        protected virtual TTenant GetTenant(Guid tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId);
            }
        }

        public void HandleEvent(EntityChangedEventData<TTenant> eventData)
        {
            _cacheManager.GetTenantCache().Remove(eventData.Entity.Id);
        }
    }
}