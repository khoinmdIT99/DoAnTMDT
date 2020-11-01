using Domain.Shop.Dto.Cart;
using Domain.Shop.Dto.Products;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.CartProduct
{
    public  class CartProductViewModel:BaseEntity
    {
		
		[MaxLength(50)]
		public string Id { get; set; }
	
		[MaxLength(50)]
		public string CartId { get; set; }
		public CartViewModel Cart { get; set; }
		
		[MaxLength(50)]
		public string ProductId { get; set; }
		public ProductViewModel Product { get; set; }
		
		public int? PriceType { get; set; }
		
		public long? Price { get; set; }
	
		public int Quantity { get; set; }
		
		public long Total { get; set; }
	}
}
