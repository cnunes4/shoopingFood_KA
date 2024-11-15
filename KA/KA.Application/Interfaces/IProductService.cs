﻿using KA.Application.DTO;

namespace KA.Application.Interfaces
{
    public  interface IProductService
    {
        Task<List<ProductDTO>?> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIDAsync(int productId);
    }
}
