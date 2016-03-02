using System.Data.Entity;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Organizations;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// Extension methods for <see cref="DbModelBuilder"/>.
    /// </summary>
    public static class AbpZeroDbModelBuilderExtensions
    {
        /// <summary>
        /// Changes prefix for ABP tables (which is "Abp" by default).
        /// Can be null/empty string to clear the prefix.
        /// </summary>
        /// <typeparam name="TTenant">The type of the tenant entity.</typeparam>
        /// <typeparam name="TRole">The type of the role entity.</typeparam>
        /// <typeparam name="TUser">The type of the user entity.</typeparam>
        /// <param name="modelBuilder">Model builder.</param>
        /// <param name="prefix">Table prefix, or null to clear prefix.</param>
        public static void ChangeAbpTablePrefix<TTenant,TRole,TUser>(this DbModelBuilder modelBuilder, string prefix)
            where TTenant : AbpTenant<TTenant, TUser>
            where TRole : AbpRole<TTenant, TUser>
            where TUser : AbpUser<TTenant, TUser>
        {
            prefix = prefix ?? "";

            modelBuilder.Entity<AuditLog>().ToTable(prefix + "AuditLogs");
            modelBuilder.Entity<BackgroundJobInfo>().ToTable(prefix + "BackgroundJobs");
            modelBuilder.Entity<Edition>().ToTable(prefix + "Editions");
            modelBuilder.Entity<FeatureSetting>().ToTable(prefix + "Features");
            modelBuilder.Entity<TenantFeatureSetting>().ToTable(prefix + "Features");
            modelBuilder.Entity<EditionFeatureSetting>().ToTable(prefix + "Features");
            modelBuilder.Entity<ApplicationLanguage>().ToTable(prefix + "Languages");
            modelBuilder.Entity<ApplicationLanguageText>().ToTable(prefix + "LanguageTexts");
            modelBuilder.Entity<NotificationInfo>().ToTable(prefix + "Notifications");
            modelBuilder.Entity<NotificationSubscriptionInfo>().ToTable(prefix + "NotificationSubscriptions");
            modelBuilder.Entity<OrganizationUnit>().ToTable(prefix + "OrganizationUnits");
            modelBuilder.Entity<PermissionSetting>().ToTable(prefix + "Permissions");
            modelBuilder.Entity<RolePermissionSetting>().ToTable(prefix + "Permissions");
            modelBuilder.Entity<UserPermissionSetting>().ToTable(prefix + "Permissions");
            modelBuilder.Entity<TRole>().ToTable(prefix + "Roles");
            modelBuilder.Entity<Setting>().ToTable(prefix + "Settings");
            modelBuilder.Entity<TTenant>().ToTable(prefix + "Tenant");
            modelBuilder.Entity<UserLogin>().ToTable(prefix + "UserLogins");
            modelBuilder.Entity<UserNotificationInfo>().ToTable(prefix + "UserNotifications");
            modelBuilder.Entity<UserOrganizationUnit>().ToTable(prefix + "UserOrganizationUnits");
            modelBuilder.Entity<UserRole>().ToTable(prefix + "UserRoles");
            modelBuilder.Entity<TUser>().ToTable(prefix + "Users");
        }
    }
}