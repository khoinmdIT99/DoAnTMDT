using Domain.Application.Dto.Menus;
using Domain.Application.IRepositories;
using Domain.Application.Services;
using Domain.Common;
using Domain.Common.Security;
using Infrastructure.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Web
{
	public class BaseAuthorizationAttribute : Attribute, IAuthorizationFilter
	{
		private UserInfo _userInfo;
		private List<CacheMenu> _cacheMenu;
		private bool Authorize(AuthorizationFilterContext actionContext)
		{
			try
			{
				var request = actionContext.HttpContext.Request;
				string token = request.Cookies[SecurityManager._securityToken];
				//Kiểm tra token có hợp lệ hay không
				bool tokenValid = SecurityManager.IsTokenValid(token, request.Headers["User-Agent"]);
				if (!tokenValid)
					return false;
				UserInfoCache userInfoCache = (UserInfoCache)actionContext.HttpContext.RequestServices.GetService(typeof(UserInfoCache));
				string userId = SecurityManager.getUserId(token);
				_userInfo = userInfoCache.GetUser(userId);
				if (_userInfo == null)
				{
					IUserRepository userRepository = (IUserRepository)actionContext.HttpContext.RequestServices.GetService(typeof(IUserRepository));
					_userInfo = (from obj in userRepository.All
								where obj.Id == userId
								select new UserInfo()
								{
									Id = obj.Id,
									DayOfBirth = obj.DayOfBirth,
									Email = obj.Email,
									FullName = obj.FullName,
									Gender = obj.Gender,
									PhoneNo = obj.PhoneNo,
									UserName = obj.UserName,
									RoleInfo = obj.UserRole.Select(p => new RoleInfo()
									{
										Id = p.RoleId,
										RoleCode = p.Role.RoleCode,
										RoleName = p.Role.RoleName
									})
								}).FirstOrDefault();
					if (_userInfo != null)
						userInfoCache.SetUser(_userInfo);
				}

				_cacheMenu = userInfoCache.GetMenuCaches();
				if (_cacheMenu == null)
				{
					IMenuRepository menuRepository = (IMenuRepository)actionContext.HttpContext.RequestServices.GetService(typeof(IMenuRepository));
					_cacheMenu = menuRepository.All.Select(p => new CacheMenu
					{
						Order = p.Order,
						Name = p.Name,
						DisplayName = p.DisplayName,
						HierarchyCode = p.HierarchyCode,
						Icon = p.Icon,
						Controller = p.Controller,
						Roles = p.MenuRoles.Select(r => r.RoleId).ToList(),
					}).OrderBy(p => p.HierarchyCode).ToList();
					userInfoCache.UpdateMenuCaches(_cacheMenu);
				}

				////Kiểm tra thông tin user trong cache, Nếu không tồn tại thì return false
				return _userInfo != null;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool ControllerAuthorize(AuthorizationFilterContext context)
		{
			string currentController = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
			if (currentController == "Default")
				return true;
			var menu = _cacheMenu.FirstOrDefault(p => p.Controller == currentController);
			if (menu != null)
			{
				return _userInfo.RoleInfo.Select(p => p.Id).Intersect(menu.Roles).Any();
			}
			return false;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!Authorize(context))
			{
				//throw new HttpResponseMessage(HttpStatusCode.Unauthorized);
				context.Result = new RedirectToActionResult("Index", "Login", new { area = "" });
				return;
			}
			if (!ControllerAuthorize(context))
			{
				context.Result = new RedirectToActionResult("Index", "Default", new { area = "Administrator" });
				return;
			}	
		}
	}
}
