using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Dto.Roles;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
	[Area("Administrator")]
	[BaseAuthorization]
	public class RolesController : BaseController
	{
		private readonly IRoleRepository _roleRepository;
        readonly ILogger<RolesController> _logger;
        readonly UserInfoCache _userInfoCache;
		public RolesController(ILogger<RolesController> logger, IRoleRepository roleRepository, UserInfoCache userInfoCache)
		{
			_logger = logger;
			this._roleRepository = roleRepository;
			this._userInfoCache = userInfoCache;
		}
		public IActionResult Index()
		{
			IEnumerable<RoleViewModel> roles = _roleRepository.GetRoleViewModels();
			return View(roles);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(RoleViewModel model)
		{
			if (ModelState.IsValid && Validate(model))
			{
				try
				{
					_roleRepository.Add(new Role()
					{
						Id = Guid.NewGuid().ToString(),
						RoleCode = model.RoleCode,
						RoleName = model.RoleName
					});
					_roleRepository.Save(RequestContext);
					_userInfoCache.RemoveMenuCaches();
					_logger.LogInformation("Create Role {0}", model.RoleName);
					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Update role {0} failed", model.RoleName);
					return View();
					throw;
				}
			}
			return View();
		}
		[HttpPost]
		public bool Delete(string id)
		{
			try
			{
				Role role = _roleRepository.Get(id);
				_roleRepository.Delete(role);
				_roleRepository.Save(RequestContext);
				_userInfoCache.RemoveMenuCaches();
				_logger.LogInformation("Delete Role {0}", role.RoleName);
				return true;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Update role {0} failed", id);
				return false;
			}

		}
		public IActionResult Update(string id)
		{

			return View(_roleRepository.GetRoleViewModel(id));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(RoleViewModel role)
		{
			if (ModelState.IsValid && Validate(role))
			{
				try
				{
					Role d = _roleRepository.All.First(s => s.Id == role.Id);
					d.RoleCode = role.RoleCode;
					d.RoleName = role.RoleName;
					_roleRepository.Save(RequestContext);
					_userInfoCache.RemoveMenuCaches();
					_logger.LogInformation("Update Role {0}", role.RoleName);
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Update role {0} failed", role.RoleName);
					return View();
				}
				return RedirectToAction("Index"); 
			}
			return View();
		}

		private bool Validate(RoleViewModel role)
		{
			var dicError = _roleRepository.Validate(role);
			if (dicError.Count == 0)
				return true;
			foreach (var item in dicError)
			{
				ModelState.AddModelError(item.Key, item.Value);
			}
			return false;
		}
	}
}
