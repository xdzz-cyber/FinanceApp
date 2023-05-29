using FluentValidation;

namespace Application.Auth.Commands.Registration.Commands;

public class RegistrationValidator : AbstractValidator<Auth.Commands.Registration.Commands.Registration>
{
    public RegistrationValidator()
    {
        // Rule for username
        RuleFor(registration => registration.UserName)
            .NotEmpty()
            .WithMessage("Username must not be empty");
        RuleFor(registration => registration.Email)
            .NotEmpty()
            .EmailAddress();
        // Rule for password
        RuleFor(registration => registration.Password)
            .NotEmpty()
            .MinimumLength(5)
            .Matches(@"^(?=.*[a-zA-Z]).{5,}$")
            .WithMessage("Password must contain at least 1 lowercase alphabetical character and must be at least 5 characters long");
    }
}