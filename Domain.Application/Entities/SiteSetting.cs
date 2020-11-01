using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
	[Table("SITE_SETTING")]
	public class SiteSetting
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[MaxLength(255)]
		[Column("PAGE_TITLE")]
		[Required]
		public string PageTitle { get; set; }
		[MaxLength(255)]
		[Required]
		[Column("PAGE_EMAIL")]
		public string PageEmail { get; set; }
		[Column("DEFAULT_PAGE_SIZE")]
		public int? DefaultPageSize { get; set; }
		[Column("PAGE_SIZE_OPTIONS")]
		public string PageSizeOptions { get; set; }
		[Column("SHOW_FOOTER")]
		public bool ShowFooter { get; set; }
		[Column("FOOTER_DATA")]
		public string FooterData { get; set; }
		[Column("ICON")]
		public string Icon { get; set; }
		[Column("LOGO")]
		public string Logo { get; set; }
	}
}
