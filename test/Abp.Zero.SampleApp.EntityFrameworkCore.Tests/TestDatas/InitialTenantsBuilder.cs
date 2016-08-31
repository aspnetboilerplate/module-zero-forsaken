using System;
using Abp.Runtime.Security;
using Abp.Zero.SampleApp.MultiTenancy;

namespace Abp.Zero.SampleApp.EntityFrameworkCore.Tests.TestDatas
{
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
            _context.Tenants.Add(
                new Tenant(Tenant.DefaultTenantName, Tenant.DefaultTenantName)
                {
                    ConnectionString = SimpleStringCipher.Instance.Encrypt($"server=localhost;database={Guid.NewGuid():N};trusted_connection=true;")
                });
            _context.SaveChanges();
        }
    }
}