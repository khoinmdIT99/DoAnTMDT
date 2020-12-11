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
        public int Total { get; set; }
        public long TotalPrice { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
        public int PaymentMethod { get; set; }
        public int ShippingMethod { get; set; }

        public int Status { get; set; }
        public List<CartProduct.CartProductViewModel> Products { get; set; }
        public CustomerViewModel Customer { get; set; }

    }
}
