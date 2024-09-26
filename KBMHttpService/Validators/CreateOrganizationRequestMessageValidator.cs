using FluentValidation;
using KBMGrpcService;

namespace KBMHttpService.Validators
{
    public class CreateOrganizationRequestMessageValidator : AbstractValidator<CreateOrganizationRequestMessage>
    {
        public CreateOrganizationRequestMessageValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Organization's name couldn't be NULL or empty!");
            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Organization's address couldn't be NULL or empty!");
        }
    }
}
