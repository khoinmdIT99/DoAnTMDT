using Domain.Shop.Dto.ProductTypes;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class ProductTypeRepository : Repository<ShopDBContext, ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }
        public IEnumerable<ProductTypeViewModel> GetProductTypeViewModels()
        {
            return this.All.Select(p => new ProductTypeViewModel
            {
                Id = p.Id,
                TypeName = p.TypeName
            }).ToList();
        }
        public ProductTypeViewModel GetProductTypeViewModel(string Id)
        {
            return this.All.Where(p => p.Id == Id).Select(prop => new ProductTypeViewModel
            {
                Id = prop.Id,
                TypeName = prop.TypeName,
            }).FirstOrDefault();
        }
    }
}
