using Application.Common.Dtos;

namespace WebMVC.Models;

public class CartPageVm
{
    public List<CoinDto> Coins { get; set; } = null!;
    public List<BudgetDto> Budgets { get; set; } = null!;
    
    public Guid SelectedBudgetId { get; set; } // Add this property to store the selected budget ID
}