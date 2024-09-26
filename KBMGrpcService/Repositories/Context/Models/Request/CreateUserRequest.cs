namespace KBMGrpcService.Repositories.Context.Models.Request
{
    public class CreateUserRequest
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
