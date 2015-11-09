using System.Globalization;
using Abp.Localization.Sources;

namespace Abp.Localization
{
    public interface IMultiTenantLocalizationSource : ILocalizationSource
    {
        string GetString(int? tenantId, string name, CultureInfo culture);
        
        string GetStringOrNull(int? tenantId, string name, CultureInfo culture, bool tryDefaults = true);
    }
}