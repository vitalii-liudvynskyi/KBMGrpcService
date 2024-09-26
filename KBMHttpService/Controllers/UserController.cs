using Grpc.Core;
using Grpc.Net.Client;
using KBMGrpcService;
using KBMGrpcService.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace KBMHttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserCore.UserCoreClient grpcClient;

        public UserController(IConfiguration configuration)
        {
            var grpcServiceUrl = configuration["ServicesLinks:KBMGrpcServiceUrl"];

            var channel = GrpcChannel.ForAddress(grpcServiceUrl);
            grpcClient = new UserCore.UserCoreClient(channel);
        }

        [HttpPost]
        [Route("/api/user")]
        [ProducesResponseType(typeof(CreateUserResponseMessage), 201)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestMessage request)
        {
            try
            {
                var response = await grpcClient.CreateUserAsync(request);
                return CreatedAtAction(nameof(CreateUser), response);
            }
            catch (UserAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/user/{id}")]
        [ProducesResponseType(typeof(UserResponseModel), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var request = new UserByIdRequestMessage { Id = id };
                var response = await grpcClient.GetUserByIdAsync(request);
                return Ok(response);
            }
            catch (UserDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpPut]
        [Route("/api/user")]
        [ProducesResponseType(typeof(UpdateUserResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestMessage request)
        {
            try
            {
                var response = await grpcClient.UpdateUserAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete]
        [Route("/api/user/{id}")]
        [ProducesResponseType(typeof(DeleteUserResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var request = new DeleteUserRequestMessage { Id = id };
                var response = await grpcClient.DeleteUserAsync(request);
                return Ok(response);
            }
            catch (UserDoesntExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/user/search")]
        [ProducesResponseType(typeof(SearchUsersResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> SearchUsers([FromQuery] SearchUsersRequestMessage request)
        {
            try
            {
                var response = await grpcClient.SearchUsersAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/organization/search-by-organization")]
        [ProducesResponseType(typeof(SearchUsersResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> SearchUsersByOrganization([FromQuery] SearchUsersByOrganizationRequestMessage request)
        {
            try
            {
                var response = await grpcClient.SearchUsersByOrganizationAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpPut]
        [Route("/api/user/associate")]
        [ProducesResponseType(typeof(AddUserToOrganizationResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> AddUserToOrganization([FromBody] AddUserToOrganizationRequestMessage request)
        {
            try
            {
                var response = await grpcClient.AddUserToOrganizationAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpPut]
        [Route("/api/user/dessociate")]
        [ProducesResponseType(typeof(RemoveUserFromOrganizationResponseMessage), 200)]
        [Consumes(Application.Json)]
        [Produces(Application.Json)]
        public async Task<IActionResult> RemoveUserFromOrganization([FromBody] RemoveUserFromOrganizationRequestMessage request)
        {
            try
            {
                var response = await grpcClient.RemoveUserFromOrganizationAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
    }
}
