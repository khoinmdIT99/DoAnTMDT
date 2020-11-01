using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("PROVINCE")]
    public class Province
    {
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }

		[Required]
		[Column("Name")]
		public string Name { get; set; }
		public ICollection<District> Districts { get; set; }
	}
}
