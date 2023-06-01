using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models;

public record LoginVm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-zA-Z]).{5,}$", ErrorMessage = "Password must contain at least 1 lowercase alphabetical character and must be at least 5 characters long")]
    public string Password { get; set; }
}