using Abp.ZeroCore.SampleApp.EntityFramework;

namespace Abp.Zero
{
    public class TestDataBuilder
    {
        private readonly SampleAppDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(SampleAppDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}
