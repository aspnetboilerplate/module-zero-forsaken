using Abp.Zero.SampleApp.MultiTenancy;

namespace Abp.Zero.SampleApp.EntityFrameworkCore.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly AppDbContext _context;

        public TestDataBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            new InitialTenantsBuilder(_context).Build();
        }
    }

    public class InitialTenantsBuilder
    {
        private readonly AppDbContext _context;

        public InitialTenantsBuilder(AppDbContext context)
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