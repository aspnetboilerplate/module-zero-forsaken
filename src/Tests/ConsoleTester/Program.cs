using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bootstrapper = new AbpBootstrapper())
            {
                bootstrapper.Initialize();

                var tester = IocManager.Instance.Resolve<Tester>();
                tester.DoTests();

                Console.WriteLine("Press enter to stop application...");
                Console.ReadLine();
            }
        }
    }

    [DependsOn(typeof(AbpZeroEntityFrameworkModule))]
    public class MyModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }

    public class Tester : ITransientDependency
    {
        private readonly IRepository<User, long> _userRepository;

        public Tester(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public void DoTests()
        {
            foreach (var user in _userRepository.GetAllList())
            {
                Console.WriteLine(user);
            }
        }
    }

    public class MyDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        public MyDbContext()
            : base("Default")
        {

        }
    }

    public class Tenant : AbpTenant<Tenant, User>
    {
        protected Tenant()
        {
            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }

    public class User : AbpUser<Tenant, User>
    {
        public override string ToString()
        {
            return string.Format("[User {0}] {1}", Id, UserName);
        }
    }

    public class Role : AbpRole<Tenant, User>
    {
        protected Role()
        {

        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }
    }
}
