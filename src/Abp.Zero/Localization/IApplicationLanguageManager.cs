using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Localization
{
    public interface IApplicationLanguageManager
    {
        Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(int? tenantId);

        Task AddAsync(ApplicationLanguage language);

        Task RemoveAsync(int? tenantId, string languageName);
    }
}