using System;
using System.Data.Entity;
using System.Reflection;
using Abp;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Modules;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
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
        private readonly MyUserStore _userStore;
        private readonly IRepository<MyEntity> _myEntityRepository;

        public Tester(IRepository<User, long> userRepository, MyUserStore userStore, IRepository<MyEntity> myEntityRepository)
        {
            _userRepository = userRepository;
            _userStore = userStore;
            _myEntityRepository = myEntityRepository;
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
        public IDbSet<MyEntity> MyEntities { get; set; }

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

    public class MyEntity : Entity
    {
        public string MyEntityProp { get; set; }
    }

    public class MyUserManager : AbpUserManager<Tenant, Role, User>
    {
        public MyUserManager(MyUserStore store)
            : base(store)
        {
        }
    }

    public class MyUserStore : AbpUserStore<Tenant, Role, User>
    {
        public MyUserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IAbpSession session)
            : base(
                userRepository,
                userLoginRepository,
                userRoleRepository,
                roleRepository,
                session)
        {
        }
    }
}
