namespace WebMVC.Models;

public class CoinsVm
{
    public List<Application.Common.Dtos.CoinDto> Coins { get; set; } = null!;
    public int TotalPages { get; set; }
    
    public int StartPage { get; set; }
   
    public int EndPage { get; set; }
    
    public int CurrentPage { get; set; }
}