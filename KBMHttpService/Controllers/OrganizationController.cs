using Grpc.Core;
using Grpc.Net.Client;
using KBMGrpcService;
using KBMGrpcService.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace KBMHttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationCore.OrganizationCoreClient grpcClient;

        public OrganizationController(IConfiguration configuration)
        {
            var grpcServiceUrl = configuration["ServicesLinks:KBMGrpcServiceUrl"];

            var channel = GrpcChannel.ForAddress(grpcServiceUrl);
            grpcClient = new OrganizationCore.OrganizationCoreClient(channel);
        }

        [HttpPost]
        [Route("/api/organization")]
        [ProducesResponseType(typeof(CreateOrganizationResponseMessage),  201)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequestMessage request)
        {
            try
            {
                var response = await grpcClient.CreateOrganizationAsync(request);
                return CreatedAtAction(nameof(CreateOrganization),response);
            }
            catch (OrganizationAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/organization/{id}")]
        [ProducesResponseType(typeof(GetOrganizationResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> GetOrganizationById(string id)
        {
            try
            {
                var request = new GetOrganizationByIdRequestMessage { Id = id };
                var response = await grpcClient.GetOrganizationByIdAsync(request);
                return Ok(response);
            }
            catch (OrganizationDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpPut]
        [Route("/api/organization")]
        [ProducesResponseType(typeof(UpdateOrganizationResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> UpdateOrganization([FromBody] UpdateOrganizationRequestMessage request)
        {
            try
            {
                var response = await grpcClient.UpdateOrganizationAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/organization/search")]
        [ProducesResponseType(typeof(SearchOrganizationsResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> SearchOrganizations([FromQuery] SearchOrganizationsQueryMessage request)
        {
            try
            {
                var response = await grpcClient.SearchOrganizationsAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete]
        [Route("/api/organization")]
        [ProducesResponseType(typeof(GetOrganizationResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> DeleteOrganization(string id)
        {
            try
            {
                var request = new DeleteOrganizationRequestMessage { Id = id };
                var response = await grpcClient.DeleteOrganizationAsync(request);
                return Ok(response);
            }
            catch (OrganizationDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
    }
}
