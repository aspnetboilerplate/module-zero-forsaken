using System.Globalization;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;

namespace Abp.Localization
{
    public class ApplicationLanguageTextManager : IApplicationLanguageTextManager, ITransientDependency
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly IRepository<ApplicationLanguageText, long> _applicationTextRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ApplicationLanguageTextManager(
            ILocalizationManager localizationManager, 
            IRepository<ApplicationLanguageText, long> applicationTextRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _localizationManager = localizationManager;
            _applicationTextRepository = applicationTextRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public string GetStringOrNull(int? tenantId, string sourceName, CultureInfo culture, string key, bool tryDefaults = true)
        {
            var source = _localizationManager.GetSource(sourceName);

            if (!(source is IMultiTenantLocalizationSource))
            {
                return source.GetStringOrNull(key, culture, tryDefaults);
            }

            return source
                .As<IMultiTenantLocalizationSource>()
                .GetStringOrNull(tenantId, key, culture, tryDefaults);
        }

        [UnitOfWork]
        public virtual async Task UpdateTextAsync(int? tenantId, string sourceName, CultureInfo culture, string key, string value)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var existingEntity = await _applicationTextRepository.FirstOrDefaultAsync(t =>
                    t.TenantId == tenantId &&
                    t.Source == sourceName &&
                    t.LanguageName == culture.Name &&
                    t.Key == key
                    );

                if (existingEntity != null)
                {
                    if (existingEntity.Value != value)
                    {
                        existingEntity.Value = value;
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                    }
                }
                else
                {
                    await _applicationTextRepository.InsertAsync(
                        new ApplicationLanguageText
                        {
                           TenantId = tenantId,
                           Source = sourceName,
                           LanguageName = culture.Name,
                           Key = key,
                           Value = value
                        });
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }
        }
    }
}