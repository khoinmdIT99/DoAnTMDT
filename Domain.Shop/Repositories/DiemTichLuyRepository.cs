using System;
using System.Collections.Generic;
using System.Text;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class DiemTichLuyRepository : Repository<ShopDBContext, DiemTichLuy>, IDiemTichLuyRepository
    {
        public DiemTichLuyRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
