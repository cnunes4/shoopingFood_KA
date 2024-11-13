using ShoopingFood.Models;

namespace ShoopingFood.Interfaces
{
    public interface IProductsService
    {
       Task<ListOfFood?> GetListOfItens();
    }

}
