using FluentValidation;
using KBMGrpcService;
using KBMHttpService.Helpers.Interfaces;

namespace KBMHttpService.Validators
{
    public class UpdateUserRequestMessageValidator : AbstractValidator<UpdateUserRequestMessage>
    {
        public UpdateUserRequestMessageValidator(IValidationHelper validationHelper)
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Id cannot be empty")
                .Must(validationHelper.IsValidGuid)
                    .WithMessage("Invalid GUID format for Id");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Name cannot be empty")
                .MinimumLength(2)
                    .WithMessage("Name must be at least 2 characters long");

            RuleFor(x => x.Username)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Username cannot be empty")
                .MinimumLength(4)
                    .WithMessage("Username must be at least 4 characters long")
                .MaximumLength(20)
                    .WithMessage("Username must be no longer than 20 characters");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Email cannot be empty")
                .EmailAddress()
                    .WithMessage("Invalid email format");
        }
    }
}
