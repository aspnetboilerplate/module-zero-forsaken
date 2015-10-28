using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Threading;

namespace Abp.Localization
{
    /// <summary>
    /// Implements <see cref="ILanguageProvider"/> to get languages from <see cref="IApplicationLanguageManager"/>.
    /// </summary>
    public class ApplicationLanguageProvider : ILanguageProvider
    {
        /// <summary>
        /// Reference to the session.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationLanguageProvider(
            IApplicationLanguageManager applicationLanguageManager, 
            ISettingManager settingManager)
        {
            _applicationLanguageManager = applicationLanguageManager;
            _settingManager = settingManager;
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// Gets the languages for current tenant.
        /// </summary>
        public IReadOnlyList<LanguageInfo> GetLanguages()
        {
            var languages = AsyncHelper.RunSync(() => _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId))
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.DisplayName)
                    .Select(l => l.ToLanguageInfo())
                    .ToList();
            
            var defaultLanguageName = _settingManager.GetSettingValue(LocalizationSettingNames.DefaultLanguage);
            if (!defaultLanguageName.IsNullOrWhiteSpace())
            {
                var defaultLanguage = languages.FirstOrDefault(l => l.Name == defaultLanguageName);
                if (defaultLanguage != null)
                {
                    defaultLanguage.IsDefault = true;
                }
            }

            return languages;
        }
    }
}