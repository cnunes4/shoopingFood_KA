using System.ComponentModel.DataAnnotations;

namespace KA.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    [Required]
    [StringLength(16, MinimumLength = 4)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
