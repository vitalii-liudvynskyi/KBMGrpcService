using KBMGrpcService.Enums;

namespace KBMGrpcService.Repositories.Context.Models.Queries
{
    public class QueryBase
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool OrderBy { get; set; }

        public string? QueryText { get; set; }

        public SortDirection Direction { get; set; }
    }
}
