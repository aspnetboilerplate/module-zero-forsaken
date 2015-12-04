using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;

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
            organizationUnit.Code = "00001";
            await OrganizationUnitRepository.InsertAsync(organizationUnit);
        }
    }
}