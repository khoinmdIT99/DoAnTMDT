using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
	[Table("MAIL_SETTING")]
	public class MailSetting
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[Column("SMTP_SERVER")]
		public string SmtpServer { get; set; }
		[Column("SMTP_PORT")]
		public int? SmtpPort { get; set; }
		[Column("SMTP_USERNAME")]
		public string SmtpUsername { get; set; }
		[Column("SMTP_PASSWORD")]
		public string SmtpPassword { get; set; }
		[Column("SENDER_EMAIL")]
		public string SenderEmail { get; set; }
	}
}
