using System.Collections.Generic;
using System.Linq;
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

        public SettingStore(IRepository<Setting, long> settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public virtual List<SettingInfo> GetAll(int? tenantId, long? userId)
        {
            return _settingRepository
                .GetAllList(s => s.TenantId == tenantId && s.UserId == userId)
                .Select(s => s.ToSettingInfo())
                .ToList();
        }

        public virtual SettingInfo GetSettingOrNull(int? tenantId, long? userId, string name)
        {
            return _settingRepository
                .FirstOrDefault(s => s.TenantId == tenantId && s.UserId == userId && s.Name == name)
                .ToSettingInfo();
        }

        public virtual void Delete(SettingInfo settingInfo)
        {
            _settingRepository.Delete(s => s.TenantId == settingInfo.TenantId && s.UserId == settingInfo.UserId && s.Name == settingInfo.Name);
        }

        public virtual void Create(SettingInfo settingInfo)
        {
            _settingRepository.Insert(settingInfo.ToSetting());
        }

        [UnitOfWork]
        public virtual void Update(SettingInfo settingInfo)
        {
            var setting = _settingRepository
                .FirstOrDefault(s => s.TenantId == settingInfo.TenantId && s.UserId == settingInfo.UserId && s.Name == settingInfo.Name);

            if (setting != null)
            {
                setting.Value = settingInfo.Value;
            }
        }
    }
}