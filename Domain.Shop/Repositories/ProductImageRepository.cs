using Domain.Shop.Dto.ProductImage;
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
    public class ProductImageRepository : Repository<ShopDBContext, ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

    }
}
