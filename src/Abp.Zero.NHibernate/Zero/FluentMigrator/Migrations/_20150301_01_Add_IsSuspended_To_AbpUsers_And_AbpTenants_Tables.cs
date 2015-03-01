using Abp.FluentMigrator.Extensions;
using FluentMigrator;

namespace Abp.Zero.FluentMigrator.Migrations
{
    [Migration(2015030101)]
    public class _20150301_01_Add_IsSuspended_To_AbpUsers_And_AbpTenants_Tables : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("AbpUsers").AddIsSuspendedColumn();
            Alter.Table("AbpTenants").AddIsSuspendedColumn();
        }
    }
}