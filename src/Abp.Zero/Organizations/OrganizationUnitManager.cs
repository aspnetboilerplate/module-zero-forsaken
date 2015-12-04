using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;

namespace Abp.Organizations
{
    /// <summary>
    /// Performs domain logic for Organization Units.
    /// </summary>
    public class OrganizationUnitManager : IDomainService
    {
        protected IRepository<OrganizationUnit, long> OrganizationUnitRepository { get; private set; }

        public OrganizationUnitManager(IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            OrganizationUnitRepository = organizationUnitRepository;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(OrganizationUnit organizationUnit)
        {
            organizationUnit.Code = await GetNextChildCodeAsync(organizationUnit.ParentId);
            await OrganizationUnitRepository.InsertAsync(organizationUnit);
        }

        public virtual async Task<string> GetNextChildCodeAsync(long? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? GetCode(parentId.Value) : null;
                return OrganizationUnit.AppendUnitCode(parentCode, OrganizationUnit.CreateUnitCode(1));
            }

            return OrganizationUnit.CalculateNextCode(lastChild.Code);
        }

        public virtual async Task<OrganizationUnit> GetLastChildOrNullAsync(long? parentId)
        {
            var children = await OrganizationUnitRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public virtual string GetCode(long id)
        {
            return OrganizationUnitRepository.Get(id).Code;
        }
    }
}