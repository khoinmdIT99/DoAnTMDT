using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
	[Table("SSO_SETTING")]
	public class SSOSetting
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[Column("ENABLE_GOOGLE_AUTH0")]
		public bool EnableGoogleAuth0 { get; set; }
		[Column("GOOGLE_CLIENT_ID")]
		public string GoogleClientId { get; set; }
		[Column("GOOGLE_CLIENT_SECRET")]
		public string GoogleClientSecret { get; set; }
	}
}
