namespace KA.Application.Interfaces
{
    public interface IAuthenticatedService 
    {
        string HashPassword(string password);
        bool ValidatePassword(string enteredPassword, string storedPassword);
    }
}
