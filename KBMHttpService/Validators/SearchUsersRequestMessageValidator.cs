using FluentValidation;
using KBMGrpcService;

namespace KBMHttpService.Validators
{
    public class SearchUsersRequestMessageValidator : AbstractValidator<SearchUsersRequestMessage>
    {
        public SearchUsersRequestMessageValidator()
        {
            RuleFor(x => x.QueryText)
                .NotNull()
                    .WithMessage("QueryText must not be null.");

            RuleFor(x => x.Page)
                .NotNull()
                    .WithMessage("Page must not be null.")
                .GreaterThan(0)
                    .WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .NotNull()
                    .WithMessage("PageSize must not be null.")
                .GreaterThan(0)
                    .WithMessage("PageSize must be greater than 0.");

            RuleFor(x => x.OrderBy)
                .NotNull()
                    .WithMessage("OrderBy must not be null.")
                .Must(orderBy => orderBy == true || orderBy == false)
                    .WithMessage("OrderBy must be either true or false.");

            RuleFor(x => x.Direction)
                .NotNull()
                    .WithMessage("Direction must not be null.")
                .Must(direction => direction == 1 || direction == 2)
                    .WithMessage("Direction must be either 1 (Ascending) or 2 (Descending).");
        }
    }
}
