using System.Globalization;
using System.Threading.Tasks;

namespace Abp.Localization
{
    public interface IApplicationLanguageTextManager
    {
        string GetStringOrNull(int? tenantId, string sourceName, CultureInfo culture, string key, bool tryDefaults = true);
    }
}