using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Abp.Localization
{
    /// <summary>
    /// Manages localization texts for host and tenants.
    /// </summary>
    public class ApplicationLanguageTextManager : IApplicationLanguageTextManager, ITransientDependency
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly IRepository<ApplicationLanguageText, Guid> _applicationTextRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLanguageTextManager"/> class.
        /// </summary>
        public ApplicationLanguageTextManager(
            ILocalizationManager localizationManager,
            IRepository<ApplicationLanguageText, Guid> applicationTextRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _localizationManager = localizationManager;
            _applicationTextRepository = applicationTextRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// Gets a localized string value.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        /// <param name="sourceName">Source name</param>
        /// <param name="culture">Culture</param>
        /// <param name="key">Localization key</param>
        /// <param name="tryDefaults">True: fallbacks to default languages if can not find in given culture</param>
        public string GetStringOrNull(Guid? tenantId, string sourceName, CultureInfo culture, string key, bool tryDefaults = true)
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

        /// <summary>
        /// Updates a localized string value.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        /// <param name="sourceName">Source name</param>
        /// <param name="culture">Culture</param>
        /// <param name="key">Localization key</param>
        /// <param name="value">New localized value.</param>
        [UnitOfWork]
        public virtual async Task UpdateStringAsync(Guid? tenantId, string sourceName, CultureInfo culture, string key, string value)
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