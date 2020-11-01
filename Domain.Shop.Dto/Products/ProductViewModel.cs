using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.Products
{
    public class ProductViewModel
    {
		public string Id { get; set; }
		[DisplayName("Slug")]
		[Required]
		[MaxLength(50)]
		public string Slug { get; set; }
		[DisplayName("Mã sản phẩm")]
		[Required]
		[MaxLength(50)]
		public string ProductCode { get; set; }
		[DisplayName("Tên sản phẩm")]
		[Required]
		[MaxLength(255)]
		public string ProductName { get; set; }
		[DisplayName("Mô tả")]
		public string Description { get; set; }
		[Required]
		public string ProductTypeId { get; set; }
		[DisplayName("Loại sản phẩm")]
		public string ProductTypeName { get; set; }
		[Required]
		public string MaterialId { get; set; }
		[DisplayName("Tên chất liệu")]
		public string MaterialName { get; set; }
		[Required]
		public string CategoryId { get; set; }
		[DisplayName("Danh mục")]
		public string CategoryName { get; set; }
	
		[DisplayName("Loại giá")]
		[Required]
		public string PriceType { get; set; }
		[DisplayName("Giá")]
		[Required]
		public long? Price { get; set; }
		public double? Star { get; set; }
		[DisplayName("Ảnh sản phẩm")]
		public List<IFormFile> ProductImages { get; set; }
		
		public List<string> DisplayImages { get; set; }
		public List<string> TagList { get; set; }

	}
}
