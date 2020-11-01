using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("PRODUCTS")]
	public class Product : BaseEntity
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[MaxLength(50)]
		[Required]
		[Column("SLUG")]
		public string Slug { get; set; }
		[MaxLength(50)]
		[Required]
		[Column("PRODUCT_CODE")]
		public string ProductCode { get; set; }
		[MaxLength(255)]
		[Required]
		[Column("PRODUCT_NAME")]
		public string ProductName { get; set; }
		[Column("DESCRIPTION")]
		public string Description { get; set; }
		public IEnumerable<ProductTag> ProductTags { get; set; }
		public string ProductTypeId { get; set; }
		public virtual ProductType ProductType { get; set; }
		public string MaterialId { get; set; }
		public virtual Material Material { get; set; }
		public string CategoryId { get; set; }
		public virtual Category Category { get; set; }
		public List<ProductImage> ProductImages { get; set; }
		[Column("PRICE_TYPE")]
		public int? PriceType { get; set; }
		[Column("PRICE")]
		public long? Price { get; set; }
		public virtual ICollection<ProductReview> ProductReviews { get; set; }
	}
}
