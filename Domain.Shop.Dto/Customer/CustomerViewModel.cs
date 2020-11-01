using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.Customer
{
    public class CustomerViewModel
    {
		public string Id { get; set; }
		[MaxLength(50, ErrorMessage = "Họ không được quá 50 ký tự")]
		[DisplayName("Họ")]
		[Required(ErrorMessage = "Họ không được để trống")]
		public string FirstName { get; set; }
		[MaxLength(50, ErrorMessage = "Tên không được quá 50 ký tự")]
		[DisplayName("Tên")]
		[Required(ErrorMessage = "Tên không được để trống")]
		public string LastName { get; set; }
		[MaxLength(50, ErrorMessage = "Mật khẩu không được quá 50 ký tự")]
		[DisplayName("Mật khẩu")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[MaxLength(50, ErrorMessage = "Mật khẩu không được quá 50 ký tự")]
		[DisplayName("Mật khẩu cũ")]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
		[MaxLength(50, ErrorMessage = "Mật khẩu không được quá 50 ký tự")]
		[DisplayName("Xác nhận mật khẩu")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage ="Mật khẩu không trùng khớp")]
		public string ConfirmPassword { get; set; }
		[DisplayName("Email")]
		[EmailAddress]
		public string Email { get; set; }
		[DisplayName("Số điện thoại")]
		public string PhoneNo { get; set; }
		[Required]
		[DisplayName("Địa chỉ")]
		public string Address { get; set; }
		[Required]
		[DisplayName("Quận/Huyện")]
		public string District { get; set; }
		[Required]
		[DisplayName("Tỉnh/Thành Phố")]
		public string Province { get; set; }
	}
}
