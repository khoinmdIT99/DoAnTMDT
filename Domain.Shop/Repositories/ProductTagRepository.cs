using Domain.Shop.Dto.ProductTag;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class ProductTagRepository : Repository<ShopDBContext, ProductTag>, IProductTagRepository
    {
        public ProductTagRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public ProductTag GetProductTagId(string id)
        {
            return this.All.Where(m => m.Id == id).FirstOrDefault();
        }

        public IEnumerable<ProductTagViewModel> GetProductTagViewModelsByProductId(string productId)
        {
            return this.All.Where(m => m.ProductId == productId).Select(s => new ProductTagViewModel
            {
                Id = s.Id,
                ProductId = s.ProductId,
                TagId = s.TagId,
                TagName = s.Tag.Name
            }).ToList();
        }

        public IEnumerable<ProductTagViewModel> GetProductTagViewModelsByTagId(string tagId)
        {
            return this.All.Where(m => m.TagId == tagId).Select(s => new ProductTagViewModel
            {
                Id = s.Id,
                ProductId = s.ProductId,
                TagId = s.TagId,
                TagName = s.Tag.Name
            }).ToList();
        }
    }
}
