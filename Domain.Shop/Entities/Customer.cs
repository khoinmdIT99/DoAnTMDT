using Domain.Application.Entities;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("CUSTOMERS")]
	public class Customer : BaseEntity
	{
		[Column("ID")]
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
		[Column("FULL_NAME")]
		[MaxLength(200)]
		public string FullName { get; set; }
        [MaxLength(255)]
		[Column("PASSWORD")]
		public string Password { get; set; }
		[MaxLength(255)]
		[Column("EMAIL")]
		public string Email { get; set; }
		[MaxLength(20)]
		[Column("PHONE_NO")]
		public string PhoneNo { get; set; }
		[Column("ADDRESS")]
		public string Address { get; set; }

		[Column("DISTRICT")]
		public string District { get; set; }
		[Column("PROVINCE")]
		public string Province { get; set; }
		

		public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; }
		public virtual ICollection<ProductReview> ProductReviews { get; set; }

	}
}
