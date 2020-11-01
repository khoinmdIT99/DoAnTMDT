using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.Dto.Order
{
    public class OrderViewModel
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public ShoppingCart.ShoppingCart ShoppingCart { get; set; }
        public int quantity { get; set; }
    }
}
