using Google.Protobuf.Collections;
using Grpc.Core;
using KBMGrpcService.Enums;
using KBMGrpcService.Exceptions;
using KBMGrpcService.Repositories.Context.Models.Queries;
using KBMGrpcService.Repositories.Context.Models.Request;
using KBMGrpcService.Repositories.Interfaces;

namespace KBMGrpcService.Controllers
{
    public class OrganizationController : OrganizationCore.OrganizationCoreBase
    {
        private readonly ILogger<OrganizationController> logger;
        private readonly IOrganizationRepository organizationRepository;

        public OrganizationController(ILogger<OrganizationController> logger, IOrganizationRepository organizationRepository)
        {
            this.logger = logger;
            this.organizationRepository = organizationRepository;
        }

        public async override Task<CreateOrganizationResponseMessage> CreateOrganization(CreateOrganizationRequestMessage request, ServerCallContext context)
        {
            if (!(await organizationRepository.Validate(request.Name, null)))
            {
                throw new OrganizationAlreadyExistsException();
            }

            var organizationId = await organizationRepository.Create(new CreateOrganizationRequest()
                {
                    Name = request.Name,
                    Address = request.Address,
                }
            );

            return new CreateOrganizationResponseMessage { Id = organizationId.ToString() };
        }

        public async override Task<GetOrganizationResponseMessage> GetOrganizationById(GetOrganizationByIdRequestMessage request, ServerCallContext context)
        {

            var organization = await organizationRepository.GetById(Guid.Parse(request.Id));

            return new GetOrganizationResponseMessage 
            {
                Organization = new OrganizationResponseModel()
                {
                    Id = organization.ID.ToString(),
                    Name = organization.Name,
                    Address = organization.Address,
                    CreatedAt = organization.CreatedAt.ToString(),
                    UpdatedAt = organization.UpdatedAt.ToString()
                }
            };
        }

        public async override Task<UpdateOrganizationResponseMessage> UpdateOrganization(UpdateOrganizationRequestMessage request, ServerCallContext context)
        {
            if (!(await organizationRepository.Validate(request.Name, Guid.Parse(request.Id))))
            {
                throw new OrganizationAlreadyExistsException();
            }

            var result = await organizationRepository.UpdateById(new UpdateOrganizationRequest
                {
                    ID = Guid.Parse(request.Id),
                    Name = request.Name,
                    Address = request.Address,
                }
            );

            return new UpdateOrganizationResponseMessage { Success = result };
        }

        public async override Task<SearchOrganizationsResponseMessage> SearchOrganizations(SearchOrganizationsQueryMessage request, ServerCallContext context)
        {
            var (organizations, count) = await organizationRepository.GetByQuery(new SearchOrganizationsQuery
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    OrderBy = request.OrderBy,
                    QueryText = request.QueryText,
                    Direction = (SortDirection)request.Direction
                }
            );

            var response = organizations.Select(org => new OrganizationResponseModel()
            {
                Id = org.ID.ToString(),
                Name = org.Name,
                Address = org.Address,
                CreatedAt = org.CreatedAt.ToString()
            });

            var result = new SearchOrganizationsResponseMessage();
            result.Organizations.AddRange(response);
            result.Quantity = count;

            return result;
        }

        public async override Task<GetOrganizationResponseMessage> DeleteOrganization(DeleteOrganizationRequestMessage request, ServerCallContext context)
        {
            var organization = await organizationRepository.DeleteById(Guid.Parse(request.Id));

            return new GetOrganizationResponseMessage
            {
                Organization = new OrganizationResponseModel
                {
                    Id = organization.ID.ToString(),
                    Name = organization.Name,
                    Address = organization.Address,
                    CreatedAt = organization.CreatedAt.ToString()
                }
            };
        }
    }
}