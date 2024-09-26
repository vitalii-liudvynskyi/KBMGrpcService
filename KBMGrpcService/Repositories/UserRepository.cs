using KBMGrpcService.Enums;
using KBMGrpcService.Exceptions;
using KBMGrpcService.Repositories.Context;
using KBMGrpcService.Repositories.Context.Models;
using KBMGrpcService.Repositories.Context.Models.Queries;
using KBMGrpcService.Repositories.Context.Models.Request;
using KBMGrpcService.Repositories.Interfaces;
using KBMGrpcService.Controllers;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> logger;
        private readonly KBMDbContext context;

        public UserRepository(ILogger<UserRepository> logger, KBMDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<bool> AddToOrganization(Guid organizationId, Guid userId)
        {
            try
            {
                var user = await context.Users
                    .Where(user => !user.IsDeleted)
                    .SingleOrDefaultAsync(user => user.ID == userId);

                if (user == null)
                {
                    logger.LogWarning($"User with ID {userId} not found.");
                    return false;
                }

                if (user.OrganizationID == organizationId)
                {
                    logger.LogInformation($"User with ID {userId} is already a member of organization {organizationId}.");
                    return false;
                }

                var organization = await context.Organizations
                    .Where(organization => !organization.IsDeleted)
                    .SingleOrDefaultAsync(org => org.ID == organizationId);

                if (organization == null)
                {
                    logger.LogWarning($"Organization with ID {organizationId} not found.");
                    return false;
                }

                user.OrganizationID = organizationId;
                user.UpdatedAt = DateTime.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation($"User with ID {userId} successfully added to organization {organizationId}.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding the user to the organization.");
                return false;
            }
        }

        public async Task<Guid> Create(CreateUserRequest request)
        {
            var id = Guid.NewGuid();

            await context.Users.AddAsync(new User()
                {
                    ID = id,
                    Name = request.Name,
                    Username = request.Username,
                    Email = request.Email,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();

            return id;
        }

        public async Task<User> Delete(Guid id)
        {
            var user = await context.Users
                .Where(user => !user.IsDeleted)
                .SingleOrDefaultAsync(user => user.ID == id);

            if (user == null)
            {
                logger.LogWarning($"User with ID {id} not found.");
                throw new UserDoesntExistException();
            }

            user.IsDeleted = true;

            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetById(Guid id)
        {

            var user = await context.Users
                .Where(user => !user.IsDeleted)
                .SingleOrDefaultAsync(user => user.ID == id);

            if (user == null)
            {
                logger.LogWarning($"User with ID {id} not found.");
                throw new UserDoesntExistException();
            }

            return user;
        }

        public async Task<(List<User>, int)> GetByOrganization(SearchUsersByOrganizationQuery query)
        {
            var searchedUsers = context.Users
                .Where(user => !user.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.QueryText))
            {
                searchedUsers = searchedUsers
                    .Where(user =>
                        (user.Name.StartsWith(query.QueryText) || user.Username.StartsWith(query.QueryText))
                        && user.OrganizationID == query.OrganizationId
                        && !user.IsDeleted);
            }

            if (query.OrderBy)
            {
                searchedUsers = query.Direction == SortDirection.Ascending
                    ? searchedUsers.OrderBy(org => org.Name)
                    : searchedUsers.OrderByDescending(org => org.Name);
            }

            return (await searchedUsers
                .Skip((query.Page * query.PageSize) - query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(),
                searchedUsers.Count());
        }

        public async Task<(List<User>, int)> GetByQuery(SearchUsersQuery query)
        {
            var searchedUsers = context.Users
                .Where(user => !user.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.QueryText))
            {
                searchedUsers = searchedUsers
                    .Where(user =>
                        !user.IsDeleted
                        && (user.Name.StartsWith(query.QueryText) || user.Username.StartsWith(query.QueryText)));
            }

            if (query.OrderBy)
            {
                searchedUsers = query.Direction == SortDirection.Ascending
                    ? searchedUsers.OrderBy(org => org.Name)
                    : searchedUsers.OrderByDescending(org => org.Name);
            }

            return (await searchedUsers
                .Skip((query.Page * query.PageSize) - query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(),
                searchedUsers.Count());
        }

        public async Task<bool> RemoveFromOrganization(Guid organizationId, Guid userId)
        {
            try
            {
                var user = await context.Users
                    .Where(user => !user.IsDeleted)
                    .SingleOrDefaultAsync(user => user.ID == userId);

                if (user == null)
                {
                    logger.LogWarning($"User with ID {userId} not found.");
                    return false;
                }

                if (user.OrganizationID != organizationId)
                {
                    logger.LogInformation($"User with ID {userId} is not a member of organization {organizationId}.");
                    return false;
                }

                var organization = await context.Organizations
                    .Where(org => !org.IsDeleted)
                    .SingleOrDefaultAsync(org => org.ID == organizationId);

                if (organization == null)
                {
                    logger.LogWarning($"Organization with ID {organizationId} not found.");
                    return false;
                }

                user.OrganizationID = null;
                user.UpdatedAt = DateTime.UtcNow;

                await context.SaveChangesAsync();
                logger.LogInformation($"User with ID {userId} successfully removed from organization {organizationId}.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing the user from the organization.");
                return false;
            }
        }

        public async Task<bool> UpdateById(UpdateUserRequest request)
        {
            var user = await context.Users
                .Where(user => !user.IsDeleted)
                .SingleOrDefaultAsync(user => user.ID == request.ID);

            if (user == null)
            {
                logger.LogWarning($"User with ID {request.ID} not found.");
                return false;
            }

            user.Name = request.Name;
            user.Username = request.Username;
            user.Email = request.Email;
            user.UpdatedAt = DateTime.UtcNow;

            context.Users.Update(user);
            return await context.SaveChangesAsync() > 0;
        }

        public  async Task<bool> Validate(string username, string email, Guid? id)
        {
            var users = context.Users
                .Where(user => !user.IsDeleted)
                .AsQueryable();

            var isUsernameTaken = users.Any(u => u.Username == username && u.ID != id);
            var isEmailTaken = users.Any(u => u.Email == username && u.ID != id);

            return !isUsernameTaken && !isEmailTaken;
        }
    }
}
