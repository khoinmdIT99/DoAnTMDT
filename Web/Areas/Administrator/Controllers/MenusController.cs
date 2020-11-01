using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Dto.Menus;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
	[Area("Administrator")]
	[BaseAuthorization]
	public class MenusController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IMenuRepository _menuRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IMenuRoleRepository _menuRoleRepository;
		private readonly UserInfoCache _userInfoCache;
		public MenusController(ILogger<MenusController> logger, IMenuRepository menuRepository, IRoleRepository roleRepository, IMenuRoleRepository menuRoleRepository, UserInfoCache userInfoCache)
		{
			this._logger = logger;
			this._menuRepository = menuRepository;
			this._roleRepository = roleRepository;
			this._menuRoleRepository = menuRoleRepository;
			this._userInfoCache = userInfoCache;
		}
		public IActionResult Index()
		{
			IEnumerable<MenuViewModel> menus = _menuRepository.GetMenuViewModels();
			menus = MenuViewModel.GetTreeMenuViewModels(menus);
			return View(menus);
		}

		private void SetComboData(string HierarchyCode = null)
		{
			var menuQuery = _menuRepository.All;
			if (!string.IsNullOrEmpty(HierarchyCode))
			{
				menuQuery = menuQuery.Where(p => !p.HierarchyCode.StartsWith(HierarchyCode)); 
			}
			ViewBag.menus = menuQuery.OrderBy(p => p.HierarchyCode).Select(p => new SelectListItem
			{
				Text = p.Name,
				Value = p.HierarchyCode
			}).ToList();
			ViewBag.roles = _roleRepository.All.Select(p => new SelectListItem
			{
				Text = p.RoleName,
				Value = p.Id
			}).ToList();

		}

		public IActionResult Create()
		{
			SetComboData();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(MenuViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.Id = Guid.NewGuid().ToString();
					Menu menu = new Menu();
					PropertyCopy.Copy(model, menu);
					menu.HierarchyCode = _menuRepository.GenerateHierarchyCode(model.ParentHierarchyCode);
					if (model.Roles != null)
					{
						menu.MenuRoles = new List<MenuRole>();
						foreach (string role in model.Roles)
						{
							menu.MenuRoles.Add(new MenuRole()
							{
								Id = Guid.NewGuid().ToString(),
								MenuId = menu.Id,
								RoleId = role
							});
						} 
					}
					_menuRepository.Add(menu);
					_roleRepository.Save(RequestContext);
					_userInfoCache.RemoveMenuCaches();
					_logger.LogInformation("Create Role {0}", model.Name);
					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Update role {0} failed", model.Name);
					SetComboData();
					return View();
					throw;
				}
			}
			SetComboData();
			return View();
		}

		[HttpPost]
		public bool Delete(string id)
		{
			try
			{
				Menu menu = _menuRepository.Single(p => p.Id == id, null, include => include.Include(q => q.MenuRoles));
				if (_menuRepository.CanDeleteMenu(menu.HierarchyCode))
				{
					_menuRepository.Delete(menu);
					_menuRoleRepository.Delete(menu.MenuRoles);
					_menuRepository.Save(RequestContext);
					_userInfoCache.RemoveMenuCaches();
					_logger.LogInformation("Delete Role {0}", menu.Name);
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Delete role {0} failed", id);
				return false;
			}

		}

		public IActionResult Update(string id)
		{
			var model = _menuRepository.GetMenuViewModel(id);
			SetComboData(model.HierarchyCode);
			model.ParentHierarchyCode = model.HierarchyCode.Substring(0, model.HierarchyCode.Length - Domain.Common.Consts.Infrastructure.HierarchyCodeLength);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(MenuViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Menu menu = _menuRepository.Single(p => p.Id == model.Id, null, include => include.Include(q => q.MenuRoles));
					List<string> excludeProperty = new List<string>() { "Id", "HierarchyCode" };
					PropertyCopy.Copy(model, menu, excludeProperty);
					string currentHierarchyParent = menu.HierarchyCode.Substring(0, menu.HierarchyCode.Length - Domain.Common.Consts.Infrastructure.HierarchyCodeLength);
					if (string.IsNullOrEmpty(model.ParentHierarchyCode) ? !string.IsNullOrEmpty(currentHierarchyParent) : model.ParentHierarchyCode != currentHierarchyParent)
					{
						var hierarchyCode = _menuRepository.GenerateHierarchyCode(model.ParentHierarchyCode);
						var childMenus = _menuRepository.GetChildMenus(menu.HierarchyCode);
						foreach (Menu item in childMenus)
						{
							item.HierarchyCode = model.ParentHierarchyCode + item.HierarchyCode.Substring(hierarchyCode.Length);
						}
						menu.HierarchyCode = hierarchyCode;
					}

					var deleteUserRole = menu.MenuRoles.Where(p => model.Roles == null || !model.Roles.Contains(p.RoleId));
					_menuRoleRepository.Delete(deleteUserRole);
					if (model.Roles != null)
					{
						var addUserRole = model.Roles.Where(p => !menu.MenuRoles.Any(q => q.RoleId == p)).Select(p => new MenuRole()
						{
							Id = Guid.NewGuid().ToString(),
							MenuId = menu.Id,
							RoleId = p
						}).ToList();
						_menuRoleRepository.Add(addUserRole);
					}
					_menuRepository.Update(menu);
					_menuRepository.Save(RequestContext);
					_userInfoCache.RemoveMenuCaches();
					_logger.LogInformation("Update Menu {0}", model.Name);
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Update Menu {0} failed", model.Name);
					SetComboData(model.HierarchyCode);
					return View();
				}
				return RedirectToAction("Index");
			}
			SetComboData(model.HierarchyCode);
			return View();
		}
	}
}
