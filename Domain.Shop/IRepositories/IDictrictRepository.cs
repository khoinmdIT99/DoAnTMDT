using Domain.Shop.Dto.Dictrict;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IDictrictRepository : IRepository<District>
    {
        IEnumerable<DictrictViewModel> GetDictrictViewModels(string ProvinceId);
        DictrictViewModel GetDictrictViewModel(string id);
    }
}
