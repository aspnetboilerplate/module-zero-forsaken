using Abp.Runtime.Session;

namespace Abp.Tests._TestBasis
{
    public class TestSession : IAbpSession
    {
        public long? UserId { get; set; }

        public int? TenantId { get; set; }
    }
}