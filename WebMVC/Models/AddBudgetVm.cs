using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models;

public record AddBudgetVm
{
    // Add properties here that will be used in the view and validation
    [Required]
    public string Name { get; set; } = null!;
    // Validation for amount
    [Required]
    [Range(1, 1000000)]
    public decimal Amount { get; set; }
    // Validation for start date
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; }
    // Validation for end date
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime EndDate { get; set; }
}