using Domain.Shop.Dto.Category;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface ICategoryRepository: IRepository<Category>
    {
		IEnumerable<CategoryViewModel> GetCategoryViewModels();
		string GenerateHierarchyCode(string HierarchyCodeParent);
		CategoryViewModel GetMenuViewModel(string id);
		List<Category> GetChildMenus(string HierarchyCode);
		bool CanDeleteMenu(string HierarchyCode);
		bool CheckSlugExist(string slug);
	}
}
