using FluentValidation;

namespace Application.Mocks.Queries.GetMock;

public class GetMockValidator : AbstractValidator<GetMock>
{
    public GetMockValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}