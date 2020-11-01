using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Application.Dto.Configuration
{
	public class SiteSettingViewModel
	{
		[Required]
		[MaxLength(255, ErrorMessage = "Tiêu đề trang không được quá 255 ký tự")]
		[DisplayName("Tiêu đề trang")]
		public string PageTitle { get; set; }
		[MaxLength(255, ErrorMessage = "Email không được quá 255 ký tự")]
		[Required]
		[DisplayName("Email")]
		public string PageEmail { get; set; }
		[DisplayName("Số bản ghi 1 trang")]
		public int? DefaultPageSize { get; set; }
		[DisplayName("Lựa chọn số bản ghi 1 trang")]
		public string PageSizeOptions { get; set; }
		[DisplayName("Hiển thị footer")]
		public bool ShowFooter { get; set; }
		[DisplayName("Nội dung footer")]
		[DataType(DataType.MultilineText)]
		public string FooterData { get; set; }
		[DisplayName("Icon")]
		public string Icon { get; set; }
		[DisplayName("Logo")]
		public string Logo { get; set; }
	}
}
