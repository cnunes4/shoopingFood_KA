namespace KA.Domain.Entities;

public class User
{
    public string Username { get; set; } = null!;

    public int UserId { get; set; }

    public string Password { get; set; } = null!;
}
