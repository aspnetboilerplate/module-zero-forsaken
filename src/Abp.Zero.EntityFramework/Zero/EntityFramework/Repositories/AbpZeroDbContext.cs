using System.Data.Entity;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.EntityFramework;
using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class AbpZeroDbContext : AbpDbContext
    {
        public virtual IDbSet<AbpUser> AbpUsers { get; set; }
        
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        
        public virtual IDbSet<AbpRole> AbpRoles { get; set; }
        
        public virtual IDbSet<UserRole> UserRoles { get; set; }
        
        public virtual IDbSet<AbpTenant> AbpTenants { get; set; }
        
        public virtual IDbSet<PermissionSetting> Permissions { get; set; }
        
        public virtual IDbSet<Setting> Settings { get; set; }

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