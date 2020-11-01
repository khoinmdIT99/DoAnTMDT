using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common.Security;
using System.Threading.Tasks;
using Domain.Application.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Domain.Shop.IRepositories;
using Domain.Shop.Dto.Dictrict;
using System.Linq.Dynamic.Core;
using Domain.Shop.Dto.Customer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Infrastructure.Common;
using Infrastructure.Database.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDictrictRepository _dictrictRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(IAccountRepository accountRepository, IServiceProvider serviceProvider,
            IProvinceRepository provinceRepository, IDictrictRepository dictrictRepository, IUserRepository userRepository)
        {
            this._accountRepository = accountRepository;
            this._serviceProvider = serviceProvider;
            this._provinceRepository = provinceRepository;
            this._dictrictRepository = dictrictRepository;
            this._userRepository = userRepository;
        }
        public IActionResult Index()
        {
            HttpRequest cookie = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            if (cookie != null)
            {
                string userId = cookie.Cookies[SecurityManager._securityToken];
                ViewBag.ListProvince = _provinceRepository.GetProvinceViewModels();
            
                if (userId != null)
                {
                    string id = SecurityManager.getUserId(userId);
                    var model = _accountRepository.GetCustomerViewModel(id);
                    if(model == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ViewBag.ListDistrict = _dictrictRepository.GetDictrictViewModels(model.Province);
                    return View(model);
                }
            }

            return View();
        }
        public JsonResult GetDictricts(string value)
        {
            try
            {
                var model = _dictrictRepository.GetDictrictViewModels(value);
                 return Json(model);     
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult Update(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = _accountRepository.Get(model.Id);
                    PropertyCopy.Copy(model, customer);
                    if (model.OldPassword != null && model.Password != null)
                    {
                        var user = _userRepository.Get(model.Id);
                        if (Security.EncryptPassword(model.OldPassword) == user.Password)
                        {
                            user.Password = Security.EncryptPassword(model.Password);
                        }
                        else
                        {
                            ViewBag.sucsess = false;
                            return View();
                        }
                        _userRepository.Update(user);
                        _userRepository.Save();
                    }
                    _accountRepository.Update(customer);
                    _accountRepository.Save();
                    ViewBag.sucsess = true;
                    return View();
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            ViewBag.sucsess = false;
            return View();
        }
    }
}
