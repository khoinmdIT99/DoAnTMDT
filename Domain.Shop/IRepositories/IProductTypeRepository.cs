using Domain.Shop.Dto.ProductTypes;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
        IEnumerable<ProductTypeViewModel> GetProductTypeViewModels();
        ProductTypeViewModel GetProductTypeViewModel(string Id);
    }
}
