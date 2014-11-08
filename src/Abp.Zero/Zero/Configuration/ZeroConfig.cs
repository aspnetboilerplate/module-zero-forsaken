namespace Abp.Zero.Configuration
{
    public class ZeroConfig
    {
        public MultiTenancyConfig MultiTenancy { get; private set; }

        public ZeroConfig(MultiTenancyConfig multiTenancy)
        {
            MultiTenancy = multiTenancy;
        }
    }
}