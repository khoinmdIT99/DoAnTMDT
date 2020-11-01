using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.Blogs
{
    public class BlogViewModel
    {
		public string Id { get; set; }

		[Required]
		public string Slug { get; set; }

		[MaxLength(255)]
		[Required]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
