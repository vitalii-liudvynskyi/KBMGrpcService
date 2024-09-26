using KBMGrpcService.Enums;
using KBMGrpcService.Exceptions;
using KBMGrpcService.Repositories.Context;
using KBMGrpcService.Repositories.Context.Models;
using KBMGrpcService.Repositories.Context.Models.Queries;
using KBMGrpcService.Repositories.Context.Models.Request;
using KBMGrpcService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ILogger<OrganizationRepository> logger;
        private readonly KBMDbContext context;

        public OrganizationRepository(ILogger<OrganizationRepository> logger, KBMDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<Guid> Create(CreateOrganizationRequest request)
        {
            var id = Guid.NewGuid();

            await context.Organizations.AddAsync(
                new Organization()
                {
                    ID = id,
                    Name = request.Name,
                    Address = request.Address,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();

            logger.LogInformation($"Organization with ID {id} created.");

            return id;
        }

        public async Task<Organization> DeleteById(Guid id)
        {
            var organization = await context.Organizations
                .Where(org => !org.IsDeleted)
                .SingleOrDefaultAsync(org => org.ID == id);

            if (organization is null)
            {
                logger.LogWarning($"Organization with ID {id} not found.");
                throw new OrganizationDoesntExistException();
            }

            organization.IsDeleted = true;
            await context.SaveChangesAsync();

            logger.LogInformation($"Organization with ID {id} deleted.");
            return organization;
        }

        public async Task<Organization> GetById(Guid id)
        {
            var organization = await context.Organizations
                .Where(org => !org.IsDeleted)
                .SingleOrDefaultAsync(org => org.ID == id);

            if (organization == null)
            {
                logger.LogWarning($"Organization with ID {id} not found.");
                throw new OrganizationDoesntExistException();
            }

            return organization;
        }

        public async Task<(List<Organization>, int)> GetByQuery(SearchOrganizationsQuery query)
        {
            var searchedOrganizations = context.Organizations
                .Where(org => !org.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.QueryText))
            {
                searchedOrganizations = searchedOrganizations
                    .Where(org => 
                        !org.IsDeleted
                        && (org.Name.StartsWith(query.QueryText) || org.Address.StartsWith(query.QueryText)));
            }

            if (query.OrderBy)
            {
                searchedOrganizations = query.Direction == SortDirection.Ascending
                    ? searchedOrganizations.OrderBy(org => org.Name)
                    : searchedOrganizations.OrderByDescending(org => org.Name);
            }

            return (await searchedOrganizations
                .Skip((query.Page * query.PageSize) - query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(),
                searchedOrganizations.Count());
        }

        public async Task<bool> UpdateById(UpdateOrganizationRequest request)
        {
            var organization = await context.Organizations
                .Where(org => !org.IsDeleted)
                .SingleOrDefaultAsync(org => org.ID == request.ID);

            if (organization is null) return false;

            organization.Name = request.Name;
            organization.Address = request.Address;
            organization.UpdatedAt = DateTime.UtcNow;

            context.Organizations.Update(organization);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Validate(string name, Guid? id)
        {
            var organizations = context.Organizations
                .Where(org => !org.IsDeleted)
                .AsQueryable();

            return !organizations.Any(org => org.Name == name && org.ID != id);
        }
    }
}