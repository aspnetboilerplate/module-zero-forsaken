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
}