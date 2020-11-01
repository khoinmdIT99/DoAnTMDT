using Domain.Shop.Dto.Category;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class CategoryRepository : Repository<ShopDBContext, Category>, ICategoryRepository
    {
		private readonly int HierarchyCodeLength = Domain.Common.Consts.Infrastructure.HierarchyCodeLength;
		private readonly string HierarchyCodeTemplate = Domain.Common.Consts.Infrastructure.HierarchyCodeTemplate;
		public CategoryRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<CategoryViewModel> GetCategoryViewModels()
        {
            return  this.All.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Slug = c.Slug,
                CategoryName = c.CategoryName,
                HierarchyCode = c.HierarchyCode
            }).OrderBy(p => p.HierarchyCode).ToList();
        }
		public string GenerateHierarchyCode(string HierarchyCodeParent)
		{
			var query = this.All;
			if (string.IsNullOrEmpty(HierarchyCodeParent))
				query = query.Where(p => p.HierarchyCode.Length == HierarchyCodeLength);
			else
				query = query.Where(p => p.HierarchyCode.StartsWith(HierarchyCodeParent) && p.HierarchyCode.Length == HierarchyCodeParent.Length + HierarchyCodeLength);
			var MaxHierarchyCode = query.OrderByDescending(p => p.HierarchyCode).Select(p => p.HierarchyCode).FirstOrDefault();
			long MaxHierarchyNumber = 0;
			if (!string.IsNullOrEmpty(MaxHierarchyCode))
			{
				MaxHierarchyCode = MaxHierarchyCode.Substring(MaxHierarchyCode.Length - HierarchyCodeLength);
			}
			if (long.TryParse(MaxHierarchyCode, out MaxHierarchyNumber))
			{
				MaxHierarchyNumber++;
			}
			return HierarchyCodeParent + string.Format(HierarchyCodeTemplate, MaxHierarchyNumber);
		}

        public CategoryViewModel GetMenuViewModel(string id)
        {
			return this.All.Where(c => c.Id == id).Select(c => new CategoryViewModel()
			{
				Id = c.Id,
				CategoryName = c.CategoryName,
				Slug = c.Slug,
				HierarchyCode = c.HierarchyCode
			}).FirstOrDefault();
        }

        public List<Category> GetChildMenus(string HierarchyCode)
        {
			return this.All.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length > HierarchyCode.Length).ToList();
		}

        public bool CanDeleteMenu(string HierarchyCode)
        {
			return !this.All.Where(c => c.HierarchyCode.StartsWith(HierarchyCode) && c.HierarchyCode.Length > HierarchyCode.Length).Any();
        }

        public bool CheckSlugExist(string slug)
        {
			if (this.All.Where(c => c.Slug == slug).Any())
			{
				return true;
			}
			return false;
        }
    }
}
