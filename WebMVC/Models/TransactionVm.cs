using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models;

public class TransactionVm
{
    // Validate date
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }
    // Validate amount
    [Required]
    [Range(1, 1000000)]
    public decimal Amount { get; set; }
    // Validate category name
    [Required]
    public string CategoryName { get; set; } = null!;
}