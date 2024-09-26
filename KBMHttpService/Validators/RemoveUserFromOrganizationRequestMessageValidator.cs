using FluentValidation;
using KBMGrpcService;
using KBMHttpService.Helpers.Interfaces;

namespace KBMHttpService.Validators
{
    public class RemoveUserFromOrganizationRequestMessageValidator : AbstractValidator<RemoveUserFromOrganizationRequestMessage>
    {
        public RemoveUserFromOrganizationRequestMessageValidator(IValidationHelper validationHelper)
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .Must(validationHelper.IsValidGuid)
                    .WithMessage("nvalid GUID format for Id")
                .NotEmpty()
                    .WithMessage("Organization's Id could not be NULL or empty!");

            RuleFor(x => x.UserId)
                .NotNull()
                .Must(validationHelper.IsValidGuid)
                    .WithMessage("nvalid GUID format for Id")
                .NotEmpty()
                    .WithMessage("Organization's Id could not be NULL or empty!");
        }
    }
}
