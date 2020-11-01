using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.ShopAddress
{
    public class ShopAddressViewModel
    {
		public string Id { get; set; }
		public string ShopSettingId { get; set; }

		[DisplayName("Trụ sở chính")]
		public bool MainAddress { get; set; }
		[DisplayName("Địa chỉ")]
		[Required]
		public string Address { get; set; }
		[DisplayName("Địa chỉ mail")]
		public string Email { get; set; }
		[DisplayName("Đường dây nóng")]
		public string Hotline { get; set; }
	}
}
