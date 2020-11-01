using Domain.Shop.Dto.Dictrict;
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
   public class DictrictRepository : Repository<ShopDBContext, District>, IDictrictRepository
    {
    
        public DictrictRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public DictrictViewModel GetDictrictViewModel(string id)
        {
            return this.All.Where(p => p.Id == id).Select(p => new DictrictViewModel()
            {
                Id = p.Id,
                Name = p.Name
            }).FirstOrDefault();
        }

        public IEnumerable<DictrictViewModel> GetDictrictViewModels(string ProvinceId)
        {
            return this.All.Where(d => d.ProvinceId == ProvinceId).Select(d => new DictrictViewModel() { 
                Id = d.Id,
                Name = d.Name
            }).ToList();
        }

      
    }
}
