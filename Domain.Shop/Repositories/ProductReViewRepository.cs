using Domain.Shop.Dto.ProductReview;
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
    class ProductReViewRepository : Repository<ShopDBContext, ProductReview>, IProductReViewRepository
    {
        public ProductReViewRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<ProductReviewViewModel> GetProductReviewViewModels(string productId)
        {
            return this.All.Where(p => p.ProductId == productId).Select(p => new ProductReviewViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Star = p.Star,
                Review = p.Review,
                CustomerId = p.CustomerId,
                ProductId = p.ProductId
            }).ToList();
        }
    }
}
