using Domain.Shop.Dto.Cart;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
   public  interface ICartRepository:IRepository<Cart>
   {
        CartViewModel GetCartViewModel(string id);
        IEnumerable<CartViewModel> GetCartViewModels();
   }
}
