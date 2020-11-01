using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.ShopSetting;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class ShopSettingController : BaseController
    {
        private readonly IShopSettingRepository _shopSettingRepository;
        public ShopSettingController(IShopSettingRepository shopSettingRepository)
        {
            _shopSettingRepository = shopSettingRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Message = TempData["WarningCreateShopSetting"];
            return View(_shopSettingRepository.GetShopSettingViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ShopSettingViewModel shopSetting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_shopSettingRepository.All.Any())
                    {
                        _shopSettingRepository.Add(new ShopSetting()
                        {
                            Id = Guid.NewGuid().ToString(),
                            PageName = shopSetting.PageName,
                            Pagetitle = shopSetting.Pagetitle,
                            PageDescription = shopSetting.PageDescription,
                            Keyword = shopSetting.Keyword,
                            TaxPercent = shopSetting.TaxPercent
                        });
                        _shopSettingRepository.Save(RequestContext);
                    }
                    else 
                    {
                        ShopSetting s = _shopSettingRepository.All.First(s => s.Id == shopSetting.Id);
                        s.PageName = shopSetting.PageName;
                        s.Pagetitle = shopSetting.Pagetitle;
                        s.PageDescription = shopSetting.PageDescription;
                        s.Keyword = shopSetting.Keyword;
                        s.TaxPercent = shopSetting.TaxPercent;
                        _shopSettingRepository.Save(RequestContext);
                    }
                    
                }
                catch (Exception )
                {
                    return View(_shopSettingRepository.GetShopSettingViewModel());
                }
                return View(_shopSettingRepository.GetShopSettingViewModel());
            }
            return View(_shopSettingRepository.GetShopSettingViewModel());
        }
    }
}
