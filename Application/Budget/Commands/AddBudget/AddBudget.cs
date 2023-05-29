using MediatR;

namespace Application.Budget.Commands.AddBudget;

public class AddBudget : IRequest<Guid>
{
    public string Name { get; set; }

    public decimal Amount { get; set; }
    
    public string UserId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}