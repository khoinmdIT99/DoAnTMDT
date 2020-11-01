using Domain.Shop.Dto.ProductTag;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IProductTagRepository : IRepository<ProductTag>
    {
        IEnumerable<ProductTagViewModel> GetProductTagViewModelsByProductId(string productId);
        IEnumerable<ProductTagViewModel> GetProductTagViewModelsByTagId(string tagId);
        ProductTag GetProductTagId(string id);
    }
}
