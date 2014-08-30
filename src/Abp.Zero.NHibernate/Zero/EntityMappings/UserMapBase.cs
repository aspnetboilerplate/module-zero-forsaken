using Abp.Domain.Entities.Mapping;
using Abp.Users;

namespace Abp.Zero.EntityMappings
{
    public abstract class UserMapBase<TUser> : EntityMap<TUser, long> where TUser : AbpUser
    {
        protected UserMapBase()
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
            Map(x => x.CreationTime);
            Map(x => x.CreatorUserId);
            Map(x => x.LastLoginTime);

            Polymorphism.Explicit();
        }
    }
}
