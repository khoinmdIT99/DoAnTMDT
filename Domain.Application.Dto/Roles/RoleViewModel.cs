using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Application.Dto.Roles
{
	public class RoleViewModel
	{
		public string Id { get; set; }
		[DisplayName("Mã vai trò")]
		[Required]
		[MaxLength(50)]
		public string RoleCode { get; set; }
		[DisplayName("Tên vai trò")]
		[Required]
		[MaxLength(50)]
		public string RoleName { get; set; }
	}
}
