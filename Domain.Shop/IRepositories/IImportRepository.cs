using System;
using System.Collections.Generic;
using System.Text;
using Domain.Shop.Entities;
using Infrastructure.Database;

namespace Domain.Shop.IRepositories
{
    public interface IImportRepository : IRepository<ImportBill>
    {
        bool IsPaymentOk(int id);
    }
}
