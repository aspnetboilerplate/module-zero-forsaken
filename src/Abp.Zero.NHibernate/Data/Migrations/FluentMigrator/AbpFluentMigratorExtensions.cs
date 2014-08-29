using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using FluentMigrator;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Abp.Data.Migrations.FluentMigrator
{
    /// <summary>
    /// This class is an extension for migration system to make easier to some common tasks.
    /// </summary>
    public static class AbpCoreModuleFluentMigratorExtensions
    {
        #region Create table

        /// <summary>
        /// Adds auditing columns to a table. See <see cref="IAudited"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithAuditColumns(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithCreationAuditColumns()
                .WithModificationAuditColumns();
        }

        /// <summary>
        /// Adds creation auditing columns to a table. See <see cref="ICreationAudited"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithCreationAuditColumns(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithCreationTimeColumn()
                .WithColumn("CreatorUserId").AsInt64().Nullable().ForeignKey("AbpUsers", "Id");
        }

        /// <summary>
        /// Adds modification auditing columns to a table. See <see cref="IModificationAudited"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithModificationAuditColumns(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("LastModificationTime").AsDateTime().Nullable()
                .WithColumn("LastModifierUserId").AsInt64().Nullable().ForeignKey("AbpUsers", "Id");
        }

        /// <summary>
        /// Ads creation auditing columns to a table. See <see cref="ICreationAudited"/>.
        /// TODO: Moved to Abp.Infrastructure.NHibernate, remove from here when updated to ABP v0.3.2!
        /// </summary>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithCreationTimeColumn(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("CreationTime").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        /// <summary>
        /// Adds TenantId column to a table as not nullable. See <see cref="AbpTenant"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithTenantId(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("TenantId").AsInt32().NotNullable().ForeignKey("AbpTenants", "Id");
        }

        /// <summary>
        /// Adds TenantId column to a table as nullable. See <see cref="AbpTenant"/>.
        /// </summary>
        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithNullableTenantId(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("TenantId").AsInt32().Nullable().ForeignKey("AbpTenants", "Id");
        }

        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithUserId(this ICreateTableWithColumnSyntax table)
        {
            return table.WithUserId("UserId");
        }

        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithNullableUserId(this ICreateTableWithColumnSyntax table)
        {
            return table.WithNullableUserId("UserId");
        }

        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithUserId(this ICreateTableWithColumnSyntax table, string columnName)
        {
            return table
                .WithColumn(columnName).AsInt64().NotNullable().ForeignKey("AbpUsers", "Id");
        }

        public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax WithNullableUserId(this ICreateTableWithColumnSyntax table, string columnName)
        {
            return table
                .WithColumn(columnName).AsInt64().Nullable().ForeignKey("AbpUsers", "Id");
        }

        #endregion

        #region Alter table

        /// <summary>
        /// Ads creation auditing columns to a table. See <see cref="ICreationAudited"/>.
        /// TODO: Moved to Abp.Infrastructure.NHibernate, remove from here when updated to ABP v0.3.2!
        /// </summary>
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddCreationTimeColumn(this IAlterTableAddColumnOrAlterColumnSyntax table)
        {
            return table
                .AddColumn("CreationTime").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddCreationAuditColumns(this IAlterTableAddColumnOrAlterColumnSyntax table)
        {
            return table
                .AddCreationTimeColumn()
                .AddColumn("CreatorUserId").AsInt64().Nullable().ForeignKey("AbpUsers", "Id");
        }

        #endregion

    }
}
