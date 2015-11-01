using System.Collections.Generic;
using System.Globalization;
using Abp.Localization.Sources;

namespace Abp.Localization
{
    public interface IMultiTenantLocalizationSource : ILocalizationSource
    {
        string GetString(int? tenantId, string name, CultureInfo culture);
        
        string GetStringOrNull(int? tenantId, string name, CultureInfo culture, bool tryDefaults = true);

        //IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId, CultureInfo culture, bool includeDefaults = true);
    }
}