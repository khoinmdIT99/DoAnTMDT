using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("SHIPPING_ADDRESS")]
	public class ShippingAddress : BaseEntity
	{
		[Column("ID")]
		[Key]
		[MaxLength(50)]
		public string Id { get; set; }
		[Column("FIRST_NAME")]
		[MaxLength(50)]
		public string FirstName { get; set; }
		[Column("LAST_NAME")]
		[MaxLength(50)]
		public string LastName { get; set; }
		public string PhoneNo { get; set; }
		[Column("ADDRESS")]
		public string Address { get; set; }
		[Column("DISTRICT")]
		public string District { get; set; }
		[Column("PROVINCE")]
		public string Province { get; set; }

	}
}
