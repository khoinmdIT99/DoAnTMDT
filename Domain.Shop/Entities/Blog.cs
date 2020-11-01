using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("BLOGS")]
	public class Blog : BaseEntity
    {
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }

		[Required]
		[Column("SLUG")]
		public string Slug { get; set; }

		[MaxLength(255)]
		[Required]
		[Column("TITLE")]
		public string Title { get; set; }

		
		[Required]
		[Column("CONTENT")]
		public string Content { get; set; }
		public virtual ICollection<BlogImage> BlogImages { get; set; }
	}
}
