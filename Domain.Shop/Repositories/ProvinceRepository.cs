using Domain.Shop.Dto.Province;
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
   public class ProvinceRepository : Repository<ShopDBContext, Province>, IProvinceRepository
    {
        public ProvinceRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<ProvinceViewModel> GetProvinceViewModels()
        {
            return this.All.Select(p =>new ProvinceViewModel(){
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
        public ProvinceViewModel GetProvinceViewModel(string id)
        {
            return this.All.Where(p => p.Id == id).Select(p => new ProvinceViewModel()
            {
                Id = p.Id,
                Name = p.Name
            }).FirstOrDefault();
        }
    }
}
