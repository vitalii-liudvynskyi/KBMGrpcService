using FluentValidation;
using KBMGrpcService;

namespace KBMHttpService.Validators
{
    public class SearchOrganizationsQueryMessageValidator : AbstractValidator<SearchOrganizationsQueryMessage>
    {
        public SearchOrganizationsQueryMessageValidator()
        {
            RuleFor(x => x.Page)
                .NotNull()
                .GreaterThan(0)
                    .WithMessage("Page could not be 0 or less!");
            RuleFor(x => x.PageSize)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                    .WithMessage("Page size could not be less then 1!");
            RuleFor(x => x.OrderBy)
                .NotNull()
                    .WithMessage("OrderBy could not be null!")
                .Must(orderBy => orderBy == true || orderBy == false)
                    .WithMessage("OrderBy must be true or false");
            RuleFor(x => x.Direction)
                .Must(direction => direction == 1 || direction == 2)
                    .WithMessage("Direction must be either 1 or 2!");
            RuleFor(x => x.QueryText)
                .NotNull()
                    .WithMessage("Text for search could not be null!");
        }
    }
}
