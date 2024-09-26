using KBMGrpcService.Repositories.Context.Models;
using KBMGrpcService.Repositories.Context.Models.Queries;
using KBMGrpcService.Repositories.Context.Models.Request;

namespace KBMGrpcService.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Guid> Create(CreateOrganizationRequest request);

        Task<Organization> GetById(Guid id);

        Task<(List<Organization>, int)> GetByQuery(SearchOrganizationsQuery query);

        Task<bool> UpdateById(UpdateOrganizationRequest request);

        Task<Organization> DeleteById(Guid id);

        Task<bool> Validate(string username, Guid? id);
    }
}
