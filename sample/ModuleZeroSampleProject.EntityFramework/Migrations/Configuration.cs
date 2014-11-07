namespace ModuleZeroSampleProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ModuleZeroSampleProject.EntityFramework.ModuleZeroSampleProjectDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ModuleZeroSampleProject";
        }

        protected override void Seed(ModuleZeroSampleProject.EntityFramework.ModuleZeroSampleProjectDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
