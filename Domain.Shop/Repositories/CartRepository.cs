using Domain.Shop.Dto.Cart;
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
    public class CartRepository : Repository<ShopDBContext, Cart>, ICartRepository
    {
        public CartRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
           
        }

        public CartViewModel GetCartViewModel(string id)
        {
           return this.All.Where(c => c.Id == id).Select(c => new CartViewModel()
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                Total = c.Total,
               CreateAt = c.CreateAt
           }).FirstOrDefault();
        }
        public IEnumerable<CartViewModel> GetCartViewModels()
        {
            var model =  this.All.Where(c => c.CustomerId != null).Select(c => new CartViewModel()
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                Total = c.Total,
                CreateAt = c.CreateAt
            }).ToList();
            return model;
        }
    }
}
