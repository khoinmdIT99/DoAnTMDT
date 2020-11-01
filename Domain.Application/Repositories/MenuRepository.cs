using Domain.Application.Dto.Menus;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Domain.Application.Repositories
{
	public class MenuRepository : Repository<ApplicationDBContext, Menu>, IMenuRepository
	{
		private readonly int HierarchyCodeLength = Domain.Common.Consts.Infrastructure.HierarchyCodeLength;
		private readonly string HierarchyCodeTemplate = Domain.Common.Consts.Infrastructure.HierarchyCodeTemplate;
		public MenuRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}

		public bool CanDeleteMenu(string HierarchyCode)
		{
			return !this.All.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length > HierarchyCode.Length).Any();
		}

		public string GenerateHierarchyCode(string HierarchyCodeParent)
		{
			var query = this.All;
			if (string.IsNullOrEmpty(HierarchyCodeParent))
				query = query.Where(p => p.HierarchyCode.Length == HierarchyCodeLength);
			else
				query = query.Where(p => p.HierarchyCode.StartsWith(HierarchyCodeParent) && p.HierarchyCode.Length == HierarchyCodeParent.Length + HierarchyCodeLength);
			var MaxHierarchyCode = query.OrderByDescending(p => p.HierarchyCode).Select(p=>p.HierarchyCode).FirstOrDefault();
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

		public List<Menu> GetChildMenus(string HierarchyCode)
		{
			return this.All.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length > HierarchyCode.Length).ToList();
		}

		public MenuViewModel GetMenuViewModel(string Id)
		{
			return this.All.Where(p=>p.Id == Id).Select(p => new MenuViewModel
			{
				Id = p.Id,
				Order = p.Order,
				Name = p.Name,
				DisplayName = p.DisplayName,
				HierarchyCode = p.HierarchyCode,
				Icon = p.Icon,
				Controller = p.Controller,
				Roles = p.MenuRoles.Select(r => r.RoleId).ToList()
			}).FirstOrDefault();
		}

		public IEnumerable<MenuViewModel> GetMenuViewModels()
		{
			var MenuList = this.All.Select(p => new MenuViewModel
			{
				Id = p.Id,
				Order = p.Order,
				Name = p.Name,
				DisplayName = p.DisplayName,
				HierarchyCode = p.HierarchyCode,
				Icon = p.Icon,
				Controller = p.Controller,
				Roles = p.MenuRoles.Select(r => r.Role.RoleName).ToList()
			}).OrderBy(p=>p.HierarchyCode);


			return MenuList;
		}
	}
}
