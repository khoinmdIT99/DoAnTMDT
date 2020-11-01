using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace Domain.Application.Dto.Menus
{
	public class MenuViewModel
	{
		[MaxLength(50)]
		public string Id { get; set; }
		[Required]
		[DisplayName("Thứ tự hiển thị")]
		public int Order { get; set; }
		[MaxLength(50)]
		[Required]
		[DisplayName("Tên menu")]
		public string Name { get; set; }
		[MaxLength(255)]
		[Required]
		[DisplayName("Tên tiêu đề")]
		public string DisplayName { get; set; }
		[MaxLength(255)]
		[DisplayName("Icon")]
		public string Icon { get; set; }
		public string HierarchyCode { get; set; }
		[DisplayName("Menu cha")]
		public string ParentHierarchyCode { get; set; }
		[Required]
		[DisplayName("Controller")]
		public string Controller { get; set; }
		[DisplayName("Vai trò")]
		public List<string> Roles { get; set; }

		public IEnumerable<MenuViewModel> Childs { get; set; }
		public IEnumerable<MenuViewModel> Path { get; set; }

		public static IEnumerable<MenuViewModel> GetTreeMenuViewModels(IEnumerable<MenuViewModel> menus, string HierarchyCode = null)
		{
			var result = new List<MenuViewModel>();
			if (string.IsNullOrEmpty(HierarchyCode))
			{
				result = menus.Where(p => p.HierarchyCode.Length == Domain.Common.Consts.Infrastructure.HierarchyCodeLength)
					.OrderBy(p => p.Order)
					.Select(p => new MenuViewModel
					{
						Id = p.Id,
						Name = p.Name,
						DisplayName = p.DisplayName,
						HierarchyCode = p.HierarchyCode,
						Icon = p.Icon,
						Order = p.Order,
						ParentHierarchyCode = HierarchyCode,
						Controller = p.Controller,
						Roles = p.Roles,
						Childs = GetTreeMenuViewModels(menus, p.HierarchyCode),
						Path = menus.Where(s => p.HierarchyCode.StartsWith(s.HierarchyCode)).OrderBy(s => s.HierarchyCode)
					}).ToList();
			}
			else
			{
				result = menus.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length - HierarchyCode.Length == Domain.Common.Consts.Infrastructure.HierarchyCodeLength)
					.OrderBy(p => p.Order)
					.Select(p => new MenuViewModel
					{
						Id = p.Id,
						Name = p.Name,
						DisplayName = p.DisplayName,
						HierarchyCode = p.HierarchyCode,
						Icon = p.Icon,
						Order = p.Order,
						ParentHierarchyCode = HierarchyCode,
						Controller = p.Controller,
						Roles = p.Roles,
						Childs = GetTreeMenuViewModels(menus, p.HierarchyCode),
						Path = menus.Where(s => p.HierarchyCode.StartsWith(s.HierarchyCode)).OrderBy(s => s.HierarchyCode)
					}).ToList();
			}
			return result;
		}
	}
}
