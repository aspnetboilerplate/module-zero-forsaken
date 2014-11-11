using System;
using Abp.Dependency;

namespace Abp.Tests._TestBasis
{
    public abstract class TestBase
    {
        protected IIocManager LocalIocManager { get; private set; }

        protected TestBase()
        {
            //Temporarily using Activator. It will be changed when ABP make IocManager's constructor public.
            LocalIocManager = (IIocManager)Activator.CreateInstance(typeof(IocManager), true);
        }
    }
}
