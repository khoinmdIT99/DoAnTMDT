using Domain.Shop.Dto.ShopSetting;
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
    public class ShopSettingRepository : Repository<ShopDBContext, ShopSetting>, IShopSettingRepository
    {
       
        public ShopSettingRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public ShopSettingViewModel GetShopSettingViewModel()
        {
            List<ShopSettingViewModel> shop = this.All.Select(m => new ShopSettingViewModel
            {
                Id = m.Id,
                PageName = m.PageName,
                Pagetitle = m.Pagetitle,
                PageDescription = m.PageDescription,
                Keyword = m.Keyword,
                TaxPercent = m.TaxPercent
            }).ToList();
            if (shop.Count() != 0)
                return shop.First();
            else
                return (new ShopSettingViewModel("", "", "", "", ""));
        }
    }
}
