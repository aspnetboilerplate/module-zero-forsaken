using System.Collections.Generic;
using Abp.Localization.Dictionaries;

namespace Abp.Localization
{
    public interface IMultiTenantLocalizationDictionary : ILocalizationDictionary
    {
        LocalizedString GetOrNull(int? tenantId, string name);

        IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId);
    }
}