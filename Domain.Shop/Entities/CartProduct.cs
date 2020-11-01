using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Domain.Shop.Entities
{
	[Table("CART_PRODUCT")]
	public class CartProduct : BaseEntity
	{
		[Column("ID")]
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
		[Column("CART_ID")]
		[MaxLength(50)]
		public string CartId { get; set; }
		public Cart Cart { get; set; }
		[Column("PRODUCT_ID")]
		[MaxLength(50)]
		public string ProductId { get; set; }
		public Product Product { get; set; }
		[Column("PRICE_TYPE")]
		public int? PriceType { get; set; }
		[Column("PRICE")]
		public long? Price { get; set; }
		[Column("QUANTITY")]
		public int Quantity { get; set; }
		[Column("TOTAL")]
		public long Total { get; set; }
		[Column("BOUGHT")]
		public bool Bought { get; set; }
	}
}
