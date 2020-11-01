using Domain.Shop.Dto.Materials;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IMaterialRepository : IRepository<Material>
    {
        IEnumerable<MaterialViewModel> GetMaterialViewModels();
        MaterialViewModel GetMaterialViewModelById(string id);
        Material GetMaterialById(string id);
    }
}
