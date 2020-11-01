using Domain.Shop.Dto.Materials;
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
    public class MaterialRepository : Repository<ShopDBContext, Material>, IMaterialRepository
    {
        public MaterialRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public Material GetMaterialById(string id)
        {
            return this.All.Where(m => m.Id == id).FirstOrDefault();
        }

        public MaterialViewModel GetMaterialViewModelById(string id)
        {
            var model = this.All.Where(m => m.Id == id).FirstOrDefault();
            MaterialViewModel viewModel = new MaterialViewModel();
            PropertyCopy.Copy(model, viewModel);
            return viewModel;
        }

        public IEnumerable<MaterialViewModel> GetMaterialViewModels()
        {
            return this.All.Select(m => new MaterialViewModel
            {
                Id = m.Id,
                MaterialName = m.MaterialName,
                Note = m.Note
            }).ToList();
        }


      
    }
}
