using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class AbpUserMap<TUser> : EntityMap<TUser, long>
        where TUser : AbpUser<TUser>
    {
        protected AbpUserMap()
            : base("AbpUsers")
        {
            Map(x => x.TenantId);
            Map(x => x.UserName);
            Map(x => x.Name);
            Map(x => x.Surname);
            Map(x => x.EmailAddress);
            Map(x => x.IsEmailConfirmed);
            Map(x => x.EmailConfirmationCode);
            Map(x => x.Password);
            Map(x => x.PasswordResetCode);
            Map(x => x.LastLoginTime);
            Map(x => x.IsActive);
            Map(x => x.AuthenticationSource);
            Map(x => x.IsLockoutEnabled);
            Map(x => x.LockoutEndDate);
            Map(x => x.AccessFailedCount);
            Map(x => x.PhoneNumber);
            Map(x => x.IsPhoneNumberConfirmed);

            this.MapFullAudited();

            Polymorphism.Explicit();
        }
    }
}
