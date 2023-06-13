using Application.Common.Dtos;

namespace WebMVC.Models;

public class CartPageVm
{
    public List<CoinDto> Coins { get; set; } = null!;
}