using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
	[Table("MENU_ROLE")]
	public class MenuRole
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[Column("MENU_ID")]
		public string MenuId { get; set; }
		[Column("ROLE_ID")]
		public string RoleId { get; set; }
		public Role Role { get; set; }
	}
}
