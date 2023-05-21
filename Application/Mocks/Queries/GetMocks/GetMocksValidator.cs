using FluentValidation;

namespace Application.Mocks.Queries.GetMocks;

public class GetMocksValidator : AbstractValidator<GetMocks>
{
    // public GetMocksValidator()
    // {
    //     RuleFor(v => v.MockId).NotEmpty().WithMessage("Id is required.");
    // }
}