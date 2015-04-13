using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.UI;
using Microsoft.AspNet.Identity;

namespace Abp.IdentityFramework
{
    public static class IdentityResultExtensions
    {
        public static void CheckResult(this IdentityResult result, ILocalizationManager localizationManager)
        {
            if (result.Succeeded)
            {
                return;
            }

            if (!result.Succeeded)
            {
                throw new UserFriendlyException(result.Errors.JoinAsString(", "));
            }
        }
    }
}
