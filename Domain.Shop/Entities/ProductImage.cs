using Infrastructure.Database.Dto;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("PRODUCT_IMAGE")]
	public class ProductImage : BaseAttachment
	{
		public ProductImage(string ProductId, UploadFileModel item) : base(item)
		{
			this.ProductId = ProductId;
		}
		public ProductImage() { }

		[Column("PRODUCT_ID")]
		[MaxLength(50)]
		public string ProductId { get; set; }
	}
}
