using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
	[Table("MENUS")]
	public class Menu
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[Column("ORDER")]
		[Required]
		public int Order { get; set; }
		[Column("NAME")]
		[MaxLength(50)]
		[Required]
		public string Name { get; set; }
		[Column("DISPLAY_NAME")]
		[MaxLength(255)]
		[Required]
		public string DisplayName { get; set; }
		[Column("ICON")]
		[MaxLength(255)]
		public string Icon { get; set; }
		[Column("HIERARCHY_CODE")]
		[Required]
		public string HierarchyCode { get; set; }
		[Column("CONTROLLER")]
		[Required]
		public string Controller { get; set; }
		public ICollection<MenuRole> MenuRoles {get;set;}
	}
}
