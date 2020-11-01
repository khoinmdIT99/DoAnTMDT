using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Dto.Users
{
	public class UserGridViewModel
	{
		public string Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PhoneNo { get; set; }
		public string ProfileImage { get; set; }
		public IEnumerable<UserRoleGridViewModel> Roles { get; set; }
	}
	public class UserRoleGridViewModel
	{
		public string RoleId { get; set; }
		public string RoleName { get; set; }
	}
}
