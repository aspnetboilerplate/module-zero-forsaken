using Abp.Zero.SampleApp.EntityFramework;
using Abp.Zero.SampleApp.MultiTenancy;

namespace Abp.Zero.SampleApp.Tests.TestDatas
{
    public class InitialTenantRoleAndUserBuilder
    {
        private readonly AppDbContext _context;

        public InitialTenantRoleAndUserBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateTenants();
        }

        private void CreateTenants()
        {
            _context.Tenants.Add(new Tenant(Tenant.DefaultTenantName, Tenant.DefaultTenantName));
            _context.SaveChanges();
        }
    }
}