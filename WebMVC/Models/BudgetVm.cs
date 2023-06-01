using System.ComponentModel.DataAnnotations;
using Application.Common.Dtos;

namespace WebMVC.Models;

public class BudgetVm
{
    [Required]
    public BudgetDto Budget { get; set; } = null!;
    [Required]
    public string CategoryName { get; set; } = null!;
    public IEnumerable<TransactionVm>? Transactions { get; set; }
    public string? TransactionsJson { get; set; }
}