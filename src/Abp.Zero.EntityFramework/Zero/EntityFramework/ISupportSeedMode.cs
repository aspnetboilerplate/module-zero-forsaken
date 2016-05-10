namespace Abp.Zero.EntityFramework
{
    public interface ISupportSeedMode
    {
        SeedMode SeedMode { get; set; }
    }
}