using Domain.Shop.Dto.ShopAddress;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class ShopAddressRepository : Repository<ShopDBContext, ShopAddress>, IShopAddressRepository
    {
        public ShopAddressRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public ShopAddress GetShopAddressById(string id)
        {
            return this.All.Where(m => m.Id == id).FirstOrDefault();
        }

        public ShopAddressViewModel GetShopAddressViewModelById(string id)
        {
            var model = this.All.Where(m => m.Id == id).FirstOrDefault();
            ShopAddressViewModel viewModel = new ShopAddressViewModel();
            PropertyCopy.Copy(model, viewModel);
            return viewModel;
        }

        public IEnumerable<ShopAddressViewModel> GetShopAddressViewModels()
        {
            return this.All.Select(m => new ShopAddressViewModel
            {
                Id = m.Id,
                ShopSettingId = m.ShopSettingId,
                MainAddress = m.MainAddress,
                Address = m.Address,
                Email = m.Email,
                Hotline = m.Hotline
            }).ToList();
        }
    }
}
