using Infrastructure.Database.Entities;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("CARTS")]
	public class Cart : BaseEntity
	{
       
        [Column("ID")]
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
		[Column("CUSTOMER_ID")]
		[MaxLength(50)]
		public string CustomerId { get; set; }
		public Customer Customer { get; set; }
		[Column("SHIPPING_METHOD")]
		public int ShippingMethod { get; set; }
		[Column("PAYMENT_METHOD")]
		public int PaymentMethod { get; set; }
		[Column("TOTAL_PRICE")]
		public long Totalprice { get; set; }
		[Column("DISCOUNT_PERCENT")]
		public long DiscountPercent { get; set; }
		[Column("DISCOUNT")]
		public long Discount { get; set; }
		[Column("TAX_PERCENT")]
		public int TaxPercent { get; set; }
		[Column("TAX")]
		public long Tax { get; set; }
		[Column("SHIPPING_FEE")]
		public long ShippingFee { get; set; }
		[Column("TOTAL")]
		public long Total { get; set; }
		[Column("SHIPPING_ADDRESS_ID")]
		[MaxLength(50)]
		public string ShippingAddressId { get; set; }
		public ShippingAddress ShippingAddress { get; set; }
		[Column("COMMENTS")]
		public string Comments { get; set; }
		[Column("STATUS")]
		public int Status { get; set; }
       
	}
}
