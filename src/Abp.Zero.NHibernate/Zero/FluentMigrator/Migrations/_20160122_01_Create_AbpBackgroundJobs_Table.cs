using FluentMigrator;

namespace Abp.Zero.FluentMigrator.Migrations
{
    [Migration(20160122)]
    public class _20160122_01_Create_AbpBackgroundJobs_Table : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("AbpBackgroundJobs")
                .WithColumn("JobType").AsString(512).NotNullable()
                .WithColumn("JobArgs").AsString(1048576).NotNullable()
                .WithColumn("TryCount").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("NextTryTime").AsDateTime().NotNullable()
                .WithColumn("LastTryTime").AsDateTime().Nullable()
                .WithColumn("IsAbandoned").AsBoolean().Nullable().WithDefaultValue(false)
                .WithColumn("Priority").AsByte().NotNullable().WithDefaultValue(15)
                .WithCreationAuditColumns();

            Create.Index("IX_IsAbandoned_NextTryTime")
                .OnTable("AbpUsers")
                .OnColumn("IsAbandoned").Ascending()
                .OnColumn("NextTryTime").Ascending()
                .WithOptions().NonClustered();
        }
    }
}