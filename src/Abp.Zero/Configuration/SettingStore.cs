using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace Abp.Configuration
{
    /// <summary>
    /// Implements <see cref="ISettingStore"/>.
    /// </summary>
    public class SettingStore : ISettingStore, ITransientDependency
    {
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingStore(
            IRepository<Setting, long> settingRepository, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _settingRepository = settingRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual async Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return 
                    (await _settingRepository.GetAllListAsync(s => s.UserId == userId))
                    .Select(s => s.ToSettingInfo())
                    .ToList();
            }
        }

        [UnitOfWork]
        public virtual async Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return 
                    (await _settingRepository.FirstOrDefaultAsync(s => s.UserId == userId && s.Name == name))
                    .ToSettingInfo();
            }
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(SettingInfo settingInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                await _settingRepository.DeleteAsync(
                    s => s.UserId == settingInfo.UserId && s.Name == settingInfo.Name
                    );
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(SettingInfo settingInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                await _settingRepository.InsertAsync(settingInfo.ToSetting());
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task UpdateAsync(SettingInfo settingInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(settingInfo.TenantId))
            {
                var setting = await _settingRepository.FirstOrDefaultAsync(
                s => s.TenantId == settingInfo.TenantId && s.UserId == settingInfo.UserId && s.Name == settingInfo.Name
                );

                if (setting != null)
                {
                    setting.Value = settingInfo.Value;
                }

                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
    }
}