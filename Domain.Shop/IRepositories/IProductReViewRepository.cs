using Domain.Shop.Dto.ProductReview;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IProductReViewRepository : IRepository<ProductReview>
    {
        IEnumerable<ProductReviewViewModel> GetProductReviewViewModels(string productId);
    }
}
