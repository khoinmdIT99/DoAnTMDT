using Domain.Common.Enums;
using Infrastructure.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Dto.Users
{
	public class UserViewModel
	{
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
		[MaxLength(50, ErrorMessage = "Họ tên không được quá 50 ký tự")]
		[DisplayName("Họ tên")]
		[Required(ErrorMessage = "Họ tên không được để trống")]
		public string FullName { get; set; }
		[MaxLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 ký tự")]
		[DisplayName("Tên đăng nhập")]
		[Required(ErrorMessage = "Tên đăng nhập không được để trống")]
		public string UserName { get; set; }
		[DisplayName("Đổi mật khẩu")]
		public bool ChangePassword { get; set; }
		[MaxLength(50, ErrorMessage = "Mật khẩu không được quá 50 ký tự")]
		[DisplayName("Mật khẩu")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[MaxLength(50, ErrorMessage = "Mật khẩu không được quá 50 ký tự")]
		[DisplayName("Xác nhận mật khẩu")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		[MaxLength(255, ErrorMessage = "Email không được quá 50 ký tự")]
		[DisplayName("Email")]
		[EmailAddress]
		[Required(ErrorMessage = "Email không được để trống")]
		public string Email { get; set; }
		[MaxLength(20, ErrorMessage = "Số điện thoại không được quá 20 ký tự")]
		[DisplayName("Số điện thoại")]
		[Phone]
		public string PhoneNo { get; set; }
		[DisplayName("Ngày sinh")]
		[DataType(DataType.Date)]
		public DateTime? DayOfBirth { get; set; }
		[DisplayName("Giới tính")]
		public int? Gender { get; set; }
		[DisplayName("Vai trò")]
		public List<string> Roles { get; set; }
		public string ProfileImage { get; set; }
	}
}
