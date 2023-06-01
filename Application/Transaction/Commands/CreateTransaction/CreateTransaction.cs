using MediatR;

namespace Application.Transaction.Commands.CreateTransaction;

public class CreateTransaction : IRequest<Guid>
{
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; }

    public Guid CategoryId { get; set; }
    
    public Guid BudgetId { get; set; }

    public string AppUserId { get; set; } = null!;
}