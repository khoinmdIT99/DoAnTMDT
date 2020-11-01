using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("DISTRICT")]
	public class District
	{
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }

		[Required]
		[Column("Name")]
		public string Name { get; set; }
		public string ProvinceId { get; set; }
		public Province Province { get; set; }
	}
}