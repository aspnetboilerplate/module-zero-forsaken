using System;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFramework.Repositories;

namespace Abp.Tests._TestBasis
{
    public class TestEfRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<TestDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public TestEfRepositoryBase()
        {

        }

        public TestEfRepositoryBase(Func<TestDbContext> dbContextFactory)
            : base(dbContextFactory)
        {

        }
    }

    public class TestEfRepositoryBase<TEntity> : TestEfRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public TestEfRepositoryBase()
        {

        }

        public TestEfRepositoryBase(Func<TestDbContext> dbContextFactory)
            : base(dbContextFactory)
        {

        }
    }
}