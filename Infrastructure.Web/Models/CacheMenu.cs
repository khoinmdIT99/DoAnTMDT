using Domain.Application.Dto.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Web.Models
{
	public class CacheMenu
	{
		public int Order { get; set; }
		public string Controller { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string Icon { get; set; }
		public string HierarchyCode { get; set; }
		public List<string> Roles { get; set; }
	}

	public class CacheMenuPathModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Icon { get; set; }
		public string HierarchyCode { get; set; }
		public string Controller { get; set; }
	}

	public class CacheMenuViewModel
	{
		public string HierarchyCode { get; set; }
		public string SelectedHierarchyCode { get; set; }
		public List<CacheMenu> menus { get; set; }
	}
}
