using System.ComponentModel.DataAnnotations;
using Application.Common.Dtos;

namespace WebMVC.Models;

public record AddTransactionVm
{
    // Add validation for CategoryId
    [Required]
    public Guid CategoryId { get; set; }
    
    // Add validation for BudgetId
    [Required]
    public Guid BudgetId { get; set; }

    // Add validation for Amount
    [Required]
    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }
    // Add validation for Date
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }
    // [Required]
    public List<CategoryDto>? Categories { get; set; } 
}