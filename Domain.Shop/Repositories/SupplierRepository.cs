using System;
using System.Collections.Generic;
using System.Text;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class SupplierRepository : Repository<ShopDBContext, Supplier>, ISupplierRepository
    {
        public SupplierRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
