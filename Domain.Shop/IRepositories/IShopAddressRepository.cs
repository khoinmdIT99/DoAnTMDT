using Domain.Shop.Dto.ShopAddress;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IShopAddressRepository : IRepository<ShopAddress>
    {
        IEnumerable<ShopAddressViewModel> GetShopAddressViewModels();
        ShopAddressViewModel GetShopAddressViewModelById(string id);
        ShopAddress GetShopAddressById(string id);
    }
}
