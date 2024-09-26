using FluentValidation;
using KBMGrpcService;

namespace KBMHttpService.Validators
{
    public class CreateUserRequestMessageValidator : AbstractValidator<CreateUserRequestMessage>
    {
        public CreateUserRequestMessageValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Name cannot be empty")
            .MinimumLength(2)
                .WithMessage("Name must be at least 2 characters long");

            RuleFor(x => x.Username)
                .NotEmpty()
                    .WithMessage("Username cannot be empty")
                .MinimumLength(4)
                    .WithMessage("Username must be at least 4 characters long")
                .MaximumLength(20)
                    .WithMessage("Username must be no longer than 20 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email cannot be empty")
                .EmailAddress()
                    .WithMessage("Invalid email format");
        }
    }
}
