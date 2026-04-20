using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "Minimum 10 characters required")]
    public string TemporaryPassword { get; set; }
}