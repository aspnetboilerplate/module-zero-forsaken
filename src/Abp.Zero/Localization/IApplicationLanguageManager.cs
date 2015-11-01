using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Abp.Localization
{
    public interface IApplicationLanguageManager
    {
        Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(int? tenantId);

        Task AddAsync(ApplicationLanguage language);

        Task RemoveAsync(int? tenantId, string languageName);

        Task UpdateAsync(int? tenantId, ApplicationLanguage language);

        Task<ApplicationLanguage> GetDefaultLanguageOrNullAsync(int? tenantId);

        Task SetDefaultLanguageAsync(int? tenantId, string languageName);
    }
}