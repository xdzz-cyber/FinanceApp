using FluentValidation;

namespace Application.Card.Commands.UpdateCard;

public class UpdateCardValidator : AbstractValidator<UpdateCard>
{
    public UpdateCardValidator()
    {
        //RuleFor(x => x.Id).NotEmpty();
        //RuleFor(x => x.StripeId).NotEmpty();
        RuleFor(x => x.UpdateAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.InitialAmount).GreaterThanOrEqualTo(0);
    }
}