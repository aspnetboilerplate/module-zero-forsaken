using System.Collections.Generic;
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
                return OrganizationUnit.AppendCode(parentCode, OrganizationUnit.CreateCode(1));
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

        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await OrganizationUnitRepository.DeleteAsync(child);
            }

            await OrganizationUnitRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(long id, long? parentId)
        {
            var ou = await OrganizationUnitRepository.GetAsync(id);
            if (ou.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);
            
            //Store old code of OU
            var oldCode = ou.Code;

            //Move OU
            ou.Code = await GetNextChildCodeAsync(parentId);
            ou.ParentId = parentId;

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = OrganizationUnit.AppendCode(ou.Code, OrganizationUnit.GetRelativeCode(child.Code, oldCode));
            }
        }

        public async Task<List<OrganizationUnit>> FindChildrenAsync(long parentId, bool recursive = false)
        {
            if (recursive)
            {
                var code = GetCode(parentId);
                return await OrganizationUnitRepository.GetAllListAsync(ou => ou.Code.StartsWith(code) && ou.Id != parentId);
            }
            else
            {
                return await OrganizationUnitRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }
        }
    }
}