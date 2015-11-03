using FluentMigrator;

namespace Abp.Zero.FluentMigrator.Migrations
{
    [Migration(2015110301)]
    public class _20151103_01_Create_AbpLanguages_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("AbpLanguages")
                .WithTenantIdAsNullable()
                .WithColumn("Name").AsString(10).NotNullable()
                .WithColumn("DisplayName").AsString(64).NotNullable()
                .WithColumn("Icon").AsString(128).Nullable()
                .WithFullAuditColumns();
        }
    }
}