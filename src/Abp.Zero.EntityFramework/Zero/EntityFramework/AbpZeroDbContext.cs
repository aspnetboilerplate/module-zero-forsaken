using System.Data.Entity;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.EntityFramework;
using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// DbContext object for  ABP zero.
    /// </summary>
    public abstract class AbpZeroDbContext<TTenant, TRole, TUser> : AbpDbContext
        where TRole : AbpRole<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        /// <summary>
        /// Tenants
        /// </summary>
        public virtual IDbSet<TTenant> Tenants { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public virtual IDbSet<TRole> Roles { get; set; }

        /// <summary>
        /// Users.
        /// </summary>
        public virtual IDbSet<TUser> Users { get; set; }

        /// <summary>
        /// User logins.
        /// </summary>
        public virtual IDbSet<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        public virtual IDbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Permissions.
        /// </summary>
        public virtual IDbSet<PermissionSetting> Permissions { get; set; }

        /// <summary>
        /// Settings.
        /// </summary>
        public virtual IDbSet<Setting> Settings { get; set; }

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        public AbpZeroDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        public AbpZeroDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TTenant>().ToTable("AbpTenants");
            modelBuilder.Entity<TRole>().ToTable("AbpRoles");
            modelBuilder.Entity<TUser>().ToTable("AbpUsers");

            modelBuilder.Entity<UserLogin>().ToTable("AbpUserLogins");
            modelBuilder.Entity<UserRole>().ToTable("AbpUserRoles");
            modelBuilder.Entity<PermissionSetting>().ToTable("AbpPermissions");
            modelBuilder.Entity<Setting>().ToTable("AbpSettings");
        }
    }
}
