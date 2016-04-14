using Abp.Localization;
using Abp.NHibernate.EntityMappings;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class ApplicationLanguageTextMap : EntityMap<ApplicationLanguageText, Guid>
    {
        public ApplicationLanguageTextMap()
            : base("AbpLanguageTexts")
        {
            Map(x => x.TenantId);
            Map(x => x.LanguageName);
            Map(x => x.Source);
            Map(x => x.Key);
            Map(x => x.Value);

            this.MapAudited();
        }
    }
}