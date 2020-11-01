using Domain.Shop.Dto.Cart;
using Domain.Shop.Dto.CartProduct;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.Dto.ShoppingCart
{
    public class ShoppingCart : CartViewModel
    {
        public List<CartProductViewModel> cartProducts { get; set; }
     
    }
}
