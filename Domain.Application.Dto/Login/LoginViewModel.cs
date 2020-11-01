using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Application.Dto.Login
{
	public class LoginViewModel
	{
		[Required]
		[DisplayName("Tên đăng nhập")]
		public string Username { get; set; }
		[Required]
		[DisplayName("Mật khẩu")]
		public string Password { get; set; }
		[DisplayName("Ghi nhớ đăng nhập")]
		public bool RememberMe { get; set; }
		public string returnUrl { get; set; }
	}
}
