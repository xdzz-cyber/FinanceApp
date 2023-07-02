using FluentValidation;

namespace Application.ApplicationUser.Queries.GetUser;

public class GetUserValidator : AbstractValidator<GetUser>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required to get user");
    }
}