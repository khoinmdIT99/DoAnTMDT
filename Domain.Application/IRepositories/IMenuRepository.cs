using Domain.Application.Dto.Menus;
using Domain.Application.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.IRepositories
{
	public interface IMenuRepository : IRepository<Menu>
	{
		//Dictionary<string, string> Validate(MenuViewModel model);
		IEnumerable<MenuViewModel> GetMenuViewModels();
		MenuViewModel GetMenuViewModel(string Id);
		string GenerateHierarchyCode(string HierarchyCodeParent);

		bool CanDeleteMenu(string HierarchyCode);
		public List<Menu> GetChildMenus(string HierarchyCode);
	}
}
