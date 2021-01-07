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
        public int BasketCount { get; set; }
        public int BuyCount { get; set; }
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
        [Display(Name = "Sản phẩm mới")]
        public bool? IsNew { get; set; }

        [Display(Name = "Đánh dấu nổi bật")]
        public bool? IsFeatured { get; set; }

        [Display(Name = "Sản phẩm bán chạy")]
        public bool? IsSpecial { get; set; }

        [Display(Name = "Kích hoạt sản phẩm")]
        public bool? Actived { get; set; }

        [Display(Name = "Lượt xem")]
        public int Views { get; set; }
        [Display(Name = "Khuyến mãi cộng thêm")]
        public double? ExtraDiscount { get; set; }
        [Display(Name = "Giảm giá (%)")]
        public int? Discount { get; set; }
        [Display(Name = "Giá bán")]
        public double? PriceAfter { get; set; }
        public DateTime? CreateAt { get; set; }
	}
}
