using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<ProductViewModel> GetProductViewModels();
        ProductViewModel GetProductViewModelById(string id);
        Product GetProductById(string id);

        IEnumerable<ProductViewModel> GetProductViewModelsByOrder(int value);
        IEnumerable<ProductViewModel> GetProductViewModelsByCategory(string categoryName);
        IEnumerable<ProductViewModel> GetProductViewModelsByPrice(int min, int max);
        IEnumerable<ProductViewModel> SortProductViewModels(string value);
    }
}
