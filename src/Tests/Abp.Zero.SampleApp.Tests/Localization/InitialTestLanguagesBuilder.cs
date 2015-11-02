using System.Linq;
using Abp.Localization;
using Abp.Zero.SampleApp.EntityFramework;
using Abp.Zero.SampleApp.MultiTenancy;

namespace Abp.Zero.SampleApp.Tests.Localization
{
    public class InitialTestLanguagesBuilder
    {
        private readonly AppDbContext _dbContext;

        public InitialTestLanguagesBuilder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Build()
        {
            InitializeLanguagesOnDatabase();
        }

        private void InitializeLanguagesOnDatabase()
        {
            var defaultTenant = _dbContext.Tenants.Single(t => t.TenancyName == Tenant.DefaultTenantName);
            
            //Host languages
            _dbContext.Languages.Add(new ApplicationLanguage { Name = "en", DisplayName = "English" });
            _dbContext.Languages.Add(new ApplicationLanguage { Name = "tr", DisplayName = "Türkçe" });
            _dbContext.Languages.Add(new ApplicationLanguage { Name = "de", DisplayName = "German" });

            //Default tenant languages
            _dbContext.Languages.Add(new ApplicationLanguage { Name = "zh-CN", DisplayName = "简体中文", TenantId = defaultTenant.Id });
        }
    }
}