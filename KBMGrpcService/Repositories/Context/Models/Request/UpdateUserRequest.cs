namespace KBMGrpcService.Repositories.Context.Models.Request
{
    public class UpdateUserRequest
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
