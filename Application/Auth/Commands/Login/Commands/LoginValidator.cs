using FluentValidation;

namespace Application.Auth.Commands.Login.Commands;

public class LoginValidator : AbstractValidator<Auth.Commands.Login.Commands.Login>
{
    public LoginValidator()
    {
        RuleFor(login => login.Email)
            .NotEmpty()
            .EmailAddress();
        // Rule for password
        RuleFor(login => login.Password)
            .NotEmpty()
            .MinimumLength(5)
            .Matches(@"^(?=.*[a-zA-Z]).{5,}$")
            .WithMessage("Password must contain at least 1 lowercase alphabetical character and must be at least 5 characters long");
    }
}