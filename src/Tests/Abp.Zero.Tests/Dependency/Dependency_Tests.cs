using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Tests._TestBasis;
using Shouldly;
using Xunit;

namespace Abp.Tests.Dependency
{
    public class Dependency_Tests : TestBase
    {
        [Fact]
        public void Should_Resolve_All_Dependencies()
        {
            LocalIocManager.Resolve<IRepository<TestUser, long>>();
            LocalIocManager.Resolve<IRepository<UserLogin, long>>();
            LocalIocManager.Resolve<IRepository<UserRole, long>>();
            LocalIocManager.Resolve<IRepository<TestRole>>();
            LocalIocManager.Resolve<IAbpSession>();
        }
    }
}
