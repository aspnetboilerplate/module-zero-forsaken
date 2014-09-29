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
    public class AbpZeroDbContext : AbpDbContext
    {
        /// <summary>
        /// Tenants
        /// </summary>
        public virtual IDbSet<AbpTenant> AbpTenants { get; set; }

        /// <summary>
        /// Users.
        /// </summary>
        public virtual IDbSet<AbpUser> AbpUsers { get; set; }
        
        /// <summary>
        /// User logins.
        /// </summary>
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        
        /// <summary>
        /// Roles.
        /// </summary>
        public virtual IDbSet<AbpRole> AbpRoles { get; set; }
        
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
            : base("Main")
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

            modelBuilder.Entity<AbpUser>().ToTable("AbpUsers");
            modelBuilder.Entity<UserLogin>().ToTable("AbpUserLogins");
            modelBuilder.Entity<AbpRole>().ToTable("AbpRoles");
            modelBuilder.Entity<UserRole>().ToTable("AbpUserRoles");
            modelBuilder.Entity<AbpTenant>().ToTable("AbpTenants");
            modelBuilder.Entity<PermissionSetting>().ToTable("AbpPermissions");
            modelBuilder.Entity<Setting>().ToTable("AbpSettings");
        }
    }
}
