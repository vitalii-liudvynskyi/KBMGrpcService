using KBMGrpcService.Repositories.Context.Models;
using KBMGrpcService.Repositories.Context.Models.Queries;
using KBMGrpcService.Repositories.Context.Models.Request;

namespace KBMGrpcService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> Create(CreateUserRequest request);

        Task<User> GetById(Guid id);

        Task<(List<User>, int)> GetByQuery(SearchUsersQuery query);

        Task<bool> UpdateById(UpdateUserRequest request);

        Task<User> Delete(Guid id);

        Task<bool> AddToOrganization(Guid organizationId, Guid userId);

        Task<bool> RemoveFromOrganization(Guid organizationId, Guid userId);

        Task<(List<User>, int)> GetByOrganization(SearchUsersByOrganizationQuery query);

        Task<bool> Validate(string username, string email, Guid? id);
    }
}
