using Domain.Shop.Dto.Customer;
using Domain.Shop.Dto.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.Dto.Cart
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public long Total { get; set; }
        public DateTime? CreateAt { get; set; }
        public List<CartProduct.CartProductViewModel> Products { get; set; }
        public CustomerViewModel Customer { get; set; }

    }
}
