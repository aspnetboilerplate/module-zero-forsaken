using System.Collections.Generic;
using Abp.Dependency;

namespace Abp.Authorization.Users.Roles
{
    public interface IUserRoleManager : ISingletonDependency
    {
        IReadOnlyList<string> GetRolesOfUser(long userId);
    }
}