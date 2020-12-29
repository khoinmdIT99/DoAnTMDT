using System;
using System.Linq;
using Domain.Shop.IRepositories;

namespace Domain.Shop.Statistic
{
    public class CustomerStatistic
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerStatistic(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public int CountCustomer()
        {
            return _customerRepository.All.ToList().Count();
        }
    }
}