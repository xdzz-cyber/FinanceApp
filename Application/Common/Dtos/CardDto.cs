namespace Application.Common.Dtos;

public class CardDto
{
    public string Id { get; set; } = null!;
    public string StripeId { get; set; } = null!;
    public decimal UpdateAmount { get; set; }
    public decimal InitialAmount { get; set; }
}