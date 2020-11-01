using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Entities;
using Domain.Shop.Dto.ShopAddress;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class ShopAddressController : BaseController
    {
        private string WarningCreateShopSetting = "Warning : Shop Setting have not been created";
        private readonly IShopAddressRepository _shopAddressRepository;
        private readonly IShopSettingRepository _shopSettingRepository;
        public ShopAddressController(IShopAddressRepository shopAddressRepository, IShopSettingRepository shopSettingRepository)
        {
            _shopSettingRepository = shopSettingRepository;
            _shopAddressRepository = shopAddressRepository;

        }
        public ActionResult Index()
        {
            return View(_shopAddressRepository.GetShopAddressViewModels());

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
            
        public ActionResult Create(ShopAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!CheckShopSetting())
                    {
                        TempData["WarningCreateShopSetting"] = WarningCreateShopSetting;
                        return RedirectToAction("Index", "ShopSetting", new { area = "Administrator" });
                    }
                    _shopAddressRepository.Add(new ShopAddress()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ShopSettingId = _shopSettingRepository.All.First().Id,
                        MainAddress = model.MainAddress,    
                        Address = model.Address,
                        Email = model.Email,
                        Hotline = model.Hotline
                    });
                    _shopAddressRepository.Save(RequestContext);
                }
                catch (Exception )
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Update(string id)
        {
            var model = _shopAddressRepository.GetShopAddressViewModelById(id);
            if (model == null)
            {
                return View();
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Update(ShopAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var shopAddress = _shopAddressRepository.GetShopAddressById(model.Id);
                    PropertyCopy.Copy(model, shopAddress);
                    _shopAddressRepository.Update(shopAddress);
                    _shopAddressRepository.Save(RequestContext);
                }
                catch (Exception )
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                var model = _shopAddressRepository.GetShopAddressById(id);
                _shopAddressRepository.Delete(model);
                _shopAddressRepository.Save(RequestContext);

                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public bool CheckShopSetting()
        {
            try
            {
                if (_shopSettingRepository.All.Count() != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception )
            {
                return false;
            }

        }
    }
}