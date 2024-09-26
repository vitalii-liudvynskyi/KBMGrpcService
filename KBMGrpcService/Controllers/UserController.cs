using Grpc.Core;
using KBMGrpcService.Enums;
using KBMGrpcService.Exceptions;
using KBMGrpcService.Repositories.Context.Models.Queries;
using KBMGrpcService.Repositories.Context.Models.Request;
using KBMGrpcService.Repositories.Interfaces;

namespace KBMGrpcService.Controllers
{
    public class UserController : UserCore.UserCoreBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserRepository userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        public async override Task<UserResponseModel> GetUserById(UserByIdRequestMessage request, ServerCallContext context)
        {
            var user = await userRepository.GetById(Guid.Parse(request.Id));

            return new UserResponseModel
            {
                Id = user.ID.ToString(),
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt.ToString(),
                UpdatedAt = user.UpdatedAt.ToString(),
                OrganizationId = user.OrganizationID.ToString()
            };
        }

        public override async Task<CreateUserResponseMessage> CreateUser(CreateUserRequestMessage request, ServerCallContext context)
        {
            if (!(await userRepository.Validate(request.Username, request.Email, null)))
            {
                throw new UserAlreadyExistsException();
            }

            var userId = await userRepository.Create(new CreateUserRequest()
                {
                    Name = request.Name,
                    Username= request.Username,
                    Email = request.Email
                }
            );

            return new CreateUserResponseMessage { Id = userId.ToString() };
        }

        public async override Task<UpdateUserResponseMessage> UpdateUser(UpdateUserRequestMessage request, ServerCallContext context)
        {
            if (!(await userRepository.Validate(request.Username, request.Email, Guid.Parse(request.Id))))
            {
                throw new UserAlreadyExistsException();
            }

            var result = await userRepository.UpdateById(new UpdateUserRequest()
                {
                    ID = Guid.Parse(request.Id),
                    Name = request.Name,
                    Username = request.Username,
                    Email = request.Email
                }
            );

            return new UpdateUserResponseMessage { Success = result };
        }

        public override async Task<DeleteUserResponseMessage> DeleteUser(DeleteUserRequestMessage request, ServerCallContext context)
        {
            var user = await userRepository.Delete(Guid.Parse(request.Id));

            return new DeleteUserResponseMessage
            {
                User = new UserResponseModel
                {
                    Id = user.ID.ToString(),
                    Name = user.Name,
                    Username = user.Username,
                    Email= user.Email,
                    CreatedAt = user.CreatedAt.ToString(),
                    OrganizationId = user.OrganizationID.ToString()
                }
            };
        }

        public override async Task<SearchUsersResponseMessage> SearchUsers(SearchUsersRequestMessage request, ServerCallContext context)
        {
            var (users, count) = await userRepository.GetByQuery(new SearchUsersQuery
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    OrderBy = request.OrderBy,
                    Direction = (SortDirection)request.Direction,
                    QueryText = request.QueryText
                }
            );

            var response = users.Select(user => new UserResponseModel
                {
                    Id = user.ID.ToString(),
                    Name = user.Name,
                    Username= user.Username,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt.ToString(),
                    UpdatedAt = user.UpdatedAt.ToString(),
                    OrganizationId = user.OrganizationID.ToString()
                }
            );

            var result = new SearchUsersResponseMessage();
            result.Users.AddRange(response);
            result.Quantity = count;

            return result;
        }

        public override async Task<SearchUsersResponseMessage> SearchUsersByOrganization(SearchUsersByOrganizationRequestMessage request, ServerCallContext context)
        {
            var (users, count) = await userRepository.GetByOrganization(new SearchUsersByOrganizationQuery
                {
                    OrganizationId = Guid.Parse(request.OrganizationId),
                    Page = request.Page,
                    PageSize = request.PageSize,
                    OrderBy = request.OrderBy,
                    Direction= (SortDirection)request.Direction,
                    QueryText = request.QueryText
                }
            );

            var response = users.Select(user => new UserResponseModel
            {
                Id = user.ID.ToString(),
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt.ToString(),
                UpdatedAt = user.UpdatedAt.ToString(),
                OrganizationId = user.OrganizationID.ToString()
            }
            );

            var result = new SearchUsersResponseMessage();
            result.Users.AddRange(response);
            result.Quantity = count;

            return result;
        }

        public override async Task<AddUserToOrganizationResponseMessage> AddUserToOrganization(AddUserToOrganizationRequestMessage request, ServerCallContext context)
        {
            var result = await userRepository.AddToOrganization(Guid.Parse(request.OrganizationId), Guid.Parse(request.UserId));

            return new AddUserToOrganizationResponseMessage { Success = result };
        }

        public override async Task<RemoveUserFromOrganizationResponseMessage> RemoveUserFromOrganization(RemoveUserFromOrganizationRequestMessage request, ServerCallContext context)
        {
            var result = await userRepository.RemoveFromOrganization(Guid.Parse(request.OrganizationId), Guid.Parse(request.UserId));

            return new RemoveUserFromOrganizationResponseMessage { Success = result };
        }
    }
}
