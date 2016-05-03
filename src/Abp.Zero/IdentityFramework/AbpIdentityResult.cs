using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Abp.IdentityFramework
{
    public class AbpIdentityResult : IdentityResult
    {
        public AbpIdentityResult()
        {
        }

        public AbpIdentityResult(IEnumerable<string> errors)
            : base(errors)
        {
        }

        public AbpIdentityResult(params string[] errors)
            : base(errors)
        {
        }

        public static AbpIdentityResult Failed(params string[] errors)
        {
            return new AbpIdentityResult(errors);
        }
    }
}