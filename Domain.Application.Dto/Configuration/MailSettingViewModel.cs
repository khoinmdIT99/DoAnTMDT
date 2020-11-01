using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Application.Dto.Configuration
{
	public class MailSettingViewModel
	{
		[DisplayName("Smtp Server")]
		[MaxLength(255, ErrorMessage = "Smtp Server không được quá 255 ký tự")]
		public string SmtpServer { get; set; }
		[DisplayName("Smtp Port")]
		public int? SmtpPort { get; set; } = 587;
		[DisplayName("Smtp Username")]
		[MaxLength(255, ErrorMessage = "Smtp Username không được quá 255 ký tự")]
		public string SmtpUsername { get; set; }
		[DisplayName("Smtp Password")]
		[MaxLength(255, ErrorMessage = "Smtp Password không được quá 255 ký tự")]
		public string SmtpPassword { get; set; }
		[DisplayName("Địa chỉ gửi mail")]
		[MaxLength(255, ErrorMessage = "Địa chỉ gửi mail không được quá 255 ký tự")]
		public string SenderEmail { get; set; }
	}
}
