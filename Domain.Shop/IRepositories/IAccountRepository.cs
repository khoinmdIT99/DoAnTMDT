using Domain.Shop.Dto.Customer;
using Domain.Shop.Dto.Dictrict;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IAccountRepository : IRepository<Customer>
    {
        CustomerViewModel GetCustomerViewModel(string id);
        
    }
}
