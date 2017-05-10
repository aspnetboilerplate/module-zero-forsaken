﻿using System.Threading.Tasks;
using Abp.Zero.SampleApp.MultiTenancy;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.MultiTenancy
{
    public class TenantManager_Tests : SampleAppTestBase
    {
        private readonly TenantManager _tenantManager;
        
        public TenantManager_Tests()
        {
            _tenantManager = Resolve<TenantManager>();
        }

        [Fact]
        public async Task Should_Not_Create_Duplicate_Tenant()
        {
            await _tenantManager.CreateAsync(new Tenant("Tenant-X", "Tenant X"));
            
            //Trying to re-create with same tenancy name

            await Assert.ThrowsAnyAsync<AbpException>(async () =>
            {
                await _tenantManager.CreateAsync(new Tenant("Tenant-X", "Tenant X"));
            });
        }
    }
}
