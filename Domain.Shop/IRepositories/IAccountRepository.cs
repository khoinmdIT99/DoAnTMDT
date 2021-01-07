using Domain.Shop.Dto.Customer;
using Domain.Shop.Dto.Dictrict;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Application.Dto.Users;
using Infrastructure.Database.DynamicLinq;

namespace Domain.Shop.IRepositories
{
    public interface IAccountRepository : IRepository<Customer>
    {
        CustomerViewModel GetCustomerViewModel(string id);
        List<CustomerViewModel> GetCustomerViewModel();
        DatatableResult<CustomerViewModel> GetCustomerViewModel(DatatableRequest request);
    }
}
