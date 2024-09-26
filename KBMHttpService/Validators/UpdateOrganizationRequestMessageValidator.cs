using FluentValidation;
using KBMGrpcService;
using KBMHttpService.Helpers.Interfaces;

namespace KBMHttpService.Validators
{
    public class UpdateOrganizationRequestMessageValidator : AbstractValidator<UpdateOrganizationRequestMessage>
    {
        public UpdateOrganizationRequestMessageValidator(IValidationHelper validationHelper)
        {
            RuleFor(x => x.Id)
                .NotNull()
                .Must(validationHelper.IsValidGuid)
                    .WithMessage("nvalid GUID format for Id")
                .NotEmpty()
                    .WithMessage("Organization's Id could not be NULL or empty!");
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Organization's name could not be NULL or empty!");
            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Organization's address could not be NULL or empty!");
        }
    }
}
