using Domain.Shop.Dto.ShopSetting;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IShopSettingRepository : IRepository<ShopSetting>
    {
        ShopSettingViewModel GetShopSettingViewModel();
    }
}
