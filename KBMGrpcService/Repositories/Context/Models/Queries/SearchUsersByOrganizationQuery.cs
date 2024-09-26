namespace KBMGrpcService.Repositories.Context.Models.Queries
{
    public class SearchUsersByOrganizationQuery : QueryBase
    {
        public Guid OrganizationId { get; set; }
    }
}
