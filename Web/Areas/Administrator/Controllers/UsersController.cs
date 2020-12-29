using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Dto.Users;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Domain.Common.Security;
using Domain.Shop.Entities;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Database.DynamicLinq;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
	[Area("Administrator")]
	[BaseAuthorization]
	public class UsersController : BaseController
	{
		private readonly IUserRepository _userRepository;
        readonly IUserRoleRepository _userRoleRepository;
        readonly IRoleRepository _roleRepository;
        readonly UserInfoCache _userInfoCache;
        private readonly IAccountRepository _accountRepository;
        readonly ILogger<UsersController> _logger;
        private readonly IPhanQuyenRepository _phanQuyenRepository;
        private readonly IQuyenRepository _quyenRepository;
		public UsersController(ILogger<UsersController> logger, IUserRepository userRepository,
			IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, UserInfoCache userInfoCache,
			IAccountRepository accountRepository, IPhanQuyenRepository phanQuyenRepository, IQuyenRepository quyenRepository)
		{
			_logger = logger;
			this._userRepository = userRepository;
			this._userRoleRepository = userRoleRepository;
			this._roleRepository = roleRepository;
			this._userInfoCache = userInfoCache;
            this._accountRepository = accountRepository;
            _phanQuyenRepository = phanQuyenRepository;
            _quyenRepository = quyenRepository;
        }
		public ActionResult Index()
		{
			ViewBag.roles = _roleRepository.All.Select(p => new SelectListItem
			{
				Text = p.RoleName,
				Value = p.Id
			}).ToList();
			return View();
		}

		[HttpPost]
		public ActionResult GetData([FromBody] DatatableRequest request)
		{
			if (request.columns != null)
			{
				foreach (var column in request.columns)
				{
					if (column.search == null)
						continue;
					switch (column.name)
					{
						case "roles":
							column.search.field = "roles.Select(r=>r.roleId)";
							column.search.Operator = FilterOperator.Contains;
							break;
						default:
							column.search.Operator = FilterOperator.Contains;
							break;
					}
				}
			}
			DatatableResult<UserGridViewModel> users = _userRepository.GetUserViewModels(request);
			return Json(users);
		}
		public ActionResult Create()
		{
			ViewBag.roles = _roleRepository.All.Select(p => new SelectListItem
			{
				Text = p.RoleName,
				Value = p.Id
			}).ToList();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(UserViewModel model)
		{
			if (ModelState.IsValid && Validate(model))
			{
				try
				{
					model.Id = Guid.NewGuid().ToString();
					User user = new User();
					PropertyCopy.Copy(model, user);
					user.Password = Security.EncryptPassword(model.Password);
					_userRepository.Add(user);
					if (model.Roles != null)
					{
						var addUserRole = model.Roles.Select(p => new UserRole()
						{
							Id = Guid.NewGuid().ToString(),
							RoleId = p,
							UserId = model.Id
						}).ToList();
						_userRoleRepository.Add(addUserRole);
					}
                    _userRepository.Save(RequestContext);
					//add customer
					Customer customer = new Customer() {
						Id = Guid.NewGuid().ToString(),
						FullName = model.FullName,
                        Email = model.UserName + "@gmail.com",
						Password = StringHelper.stringToSHA512(model.Password),
						CreateBy = DateTime.Now.ToShortDateString(),
						TinhTrang = "Không khoá",
						Avatar = "avatar1.png"
					};
                    await _accountRepository.AddAsync(customer);
                    await _accountRepository.SaveAsync(RequestContext);

					var quyen = _quyenRepository.All.FirstOrDefault(x => x.TenQuyen.Contains("Nhân viên"));

					if (quyen != null)
                    {
                        var maquyen = quyen.MaQuyen;
                        PhanQuyen phanQuyen = new PhanQuyen()
                        {
                            MaQuyen = maquyen,
                            MaTaiKhoan = customer.Id
                        };
                        await _phanQuyenRepository.AddAsync(phanQuyen);
                        await _phanQuyenRepository.SaveAsync();
                    }

                    _logger.LogInformation("Create User {0} - ID: {1}", user.UserName, user.Id);
					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Update user {0} failed", model.UserName);
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
                User user = _userRepository.Single(p => p.Id == id, null, include => include.Include(q => q.UserRole));
                _userRoleRepository.Delete(user.UserRole);
                _userRepository.Delete(user);
                _userRepository.Save(RequestContext);
                //delete customer
                Customer customer = _accountRepository.All.Where(c => c.Id == id).Include(c => c.ProductReviews).Include(c => c.CustomerFeedbacks).FirstOrDefault();
                if (customer != null)
                {
                    _accountRepository.Delete(customer.CustomerFeedbacks);
                    _accountRepository.Delete(customer.CustomerFeedbacks);
                    _accountRepository.Delete(customer);
                }
                _accountRepository.Save(RequestContext);
				_userInfoCache.RemoveUser(id);
				_logger.LogInformation("Delete User {0} - ID: {1}", user.UserName, user.Id);
				return true;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Update user {0} failed", id);
				return false;
			}

		}
		public ActionResult Update(string id)
		{
			ViewBag.roles = _roleRepository.All.Select(p => new SelectListItem
			{
				Text = p.RoleName,
				Value = p.Id
			}).ToList();
			return View(_userRepository.GetUserViewModel(id));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(UserViewModel model)
		{
			if (ModelState.IsValid && Validate(model))
			{
				try
				{
					User user = _userRepository.Single(p => p.Id == model.Id, null, include => include.Include(q => q.UserRole));
					List<string> excludeProperties = new List<string>() { "Id", "Password" };
					PropertyCopy.Copy(model, user, excludeProperties);
					if (model.ChangePassword)
					{
						user.Password = Security.EncryptPassword(model.Password);
					}
					var deleteUserRole = user.UserRole.Where(p => model.Roles == null || !model.Roles.Contains(p.RoleId));
					_userRoleRepository.Delete(deleteUserRole);
					if (model.Roles != null)
					{
						var addUserRole = model.Roles.Where(p => !user.UserRole.Any(q => q.RoleId == p)).Select(p => new UserRole()
						{
							Id = Guid.NewGuid().ToString(),
							RoleId = p,
							UserId = user.Id
						}).ToList();
						_userRoleRepository.Add(addUserRole);
					}

					_userRepository.Update(user);
					_userRepository.Save(RequestContext);
					_logger.LogInformation("Update User {0} - ID: {1}", user.UserName, user.Id);
				}
				catch (Exception e)
				{
					_logger.LogError(e, "Update user {0} failed", model.Id);
					return View();
				}
				return RedirectToAction("Index");
			}
			return View(model);
		}

		private bool Validate(UserViewModel user)
		{
			bool result = true;
			var dicError = _userRepository.Validate(user);
			foreach (var item in dicError)
			{
				ModelState.AddModelError(item.Key, item.Value);
				result = false;
			}

			if (user.ChangePassword)
			{
				if (string.IsNullOrEmpty(user.Password))
				{
					ModelState.AddModelError("Password", "Mật khẩu không được để trống");
					result = false;
				}
				if (user.Password != user.ConfirmPassword)
				{
					ModelState.AddModelError("ConfirmPassword", "Mật khẩu không khớp nhau");
					result = false;
				}
			}
			return result;
		}
	}
}
