using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class ImportRepository: Repository<ShopDBContext,ImportBill>, IImportRepository
    {
        public ImportRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsPaymentOk(int id)
        {
            bool isPok = true;
            var payment = Get(id);
            if (payment != null)
            {
                if (string.Equals(payment.EndDate.ToString(CultureInfo.InvariantCulture), null,
                    StringComparison.Ordinal))
                {
                    isPok = false;
                }
            }
            return isPok;
        }
    }
}
