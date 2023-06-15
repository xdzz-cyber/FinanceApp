namespace WebMVC.Models;

public class AddCoinsVm
{
    public List<CoinVm> Coins { get; set; } = null!;

    public Guid budgetId { get; set; }
}