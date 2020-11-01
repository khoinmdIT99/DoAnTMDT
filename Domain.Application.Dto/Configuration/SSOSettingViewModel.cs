using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Application.Dto.Configuration
{
	public class SSOSettingViewModel
	{
		[DisplayName("Bật đăng nhập qua Google")]
		public bool EnableGoogleAuth0 { get; set; }
		[MaxLength(255, ErrorMessage = "Google Client Id không được quá 255 ký tự")]
		[DisplayName("Google Client Id")]
		public string GoogleClientId { get; set; }
		[MaxLength(255, ErrorMessage = "Google Client Secret không được quá 255 ký tự")]
		[DisplayName("Google Client Secret")]
		public string GoogleClientSecret { get; set; }
	}
}
