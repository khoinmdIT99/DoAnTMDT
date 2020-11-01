using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("SHOP_ADDRESS")]
	public class ShopAddress : BaseEntity
	{
		[Column("ID")]
		[Required]
		public string Id { get; set; }
		[Column("SHOP_SETTING_ID")]
		[Required]
		public string ShopSettingId { get; set; }
		[Column("IS_MAIN_ADDRESS")]
		public bool MainAddress { get; set; }
		[Column("ADDRESS")]
		[Required]
		public string Address { get; set; }
		[Column("EMAIL")]
		public string Email { get; set; }
		[Column("HOTLINE")]
		public string Hotline { get; set; }
	}
}
