using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.Materials
{
	public class MaterialViewModel
	{
		public string Id { get; set; }
		[DisplayName("Tên chất liệu")]
		[Required]
		[MaxLength(50)]
		public string MaterialName { get; set; }
		[DisplayName("Lưu ý khi sử dụng")]
		[MaxLength(50)]
		public string Note { get; set; }
	}
}
