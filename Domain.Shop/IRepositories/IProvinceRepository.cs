using Domain.Shop.Dto.Province;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IProvinceRepository : IRepository<Province>
    {
        IEnumerable<ProvinceViewModel> GetProvinceViewModels();
        ProvinceViewModel GetProvinceViewModel(string id);
    }
}
