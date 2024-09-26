namespace KBMGrpcService.Repositories.Context.Models.Request
{
    public class UpdateOrganizationRequest
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }
}
