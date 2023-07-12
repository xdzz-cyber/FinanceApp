using Application.Common.Dtos;
using MediatR;

namespace Application.Transaction.Queries.GetTransactions;

public class GetTransactions : IRequest<IEnumerable<TransactionDto>>
{
    public Guid BudgetId { get; set; }
}