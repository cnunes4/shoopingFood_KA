using ShoopingFood.Models;

namespace ShoopingFood.Interfaces
{
    public interface ILoginService
    {
       Task<UserResponse> Login(User user);
    }

}
