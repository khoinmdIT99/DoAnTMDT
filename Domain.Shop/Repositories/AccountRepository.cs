using Domain.Shop.Dto.Customer;
using Domain.Shop.Dto.Dictrict;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class AccountRepository : Repository<ShopDBContext, Customer>, IAccountRepository
    {
        public AccountRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public CustomerViewModel GetCustomerViewModel(string id)
        {
           
            var model = this.All.FirstOrDefault(c => c.Id == id);
            if(model != null)
            {
                return new CustomerViewModel()
                {
                    Id = model.Id,
                    Email = model.Email,
                    FirstName = model.FullName,
                    PhoneNo = model.PhoneNo,
                    Address = model.Address,
                    District = model.District,
                    Province = model.Province
                };
            }
            return null;
        }
        
    }
}
