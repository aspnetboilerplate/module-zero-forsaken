namespace Abp.Zero.Configuration
{
    /// <summary>
    /// Configuration options for zero module.
    /// </summary>
    public class ZeroConfig
    {
        /// <summary>
        /// Multi tenancy configuration.
        /// </summary>
        public MultiTenancyConfig MultiTenancy { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="multiTenancy">Multi tenancy configuration</param>
        public ZeroConfig(MultiTenancyConfig multiTenancy)
        {
            MultiTenancy = multiTenancy;
        }
    }
}