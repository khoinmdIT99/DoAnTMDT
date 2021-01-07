using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shop.History;
using Domain.Shop.IRepositories;

namespace Domain.Shop.Statistic
{ 
    public class ProductStatistic
    {
        private readonly IProductRepository _iProductRepository;

        public ProductStatistic(IProductRepository iProductRepository)
        {
            _iProductRepository = iProductRepository;
        }
        //public static List<ProductAvailabilityCheck> GetProductAvailabilityHistory(string productID)
        //{
        //    return _context.ProductAvailabilityCheck.Where(m => m.ProductID == productID).OrderBy(m => m.Date).ToList();
        //}

        public double GetCurrentTotalProductValue()
        {
            double total = _iProductRepository.All.ToList().Sum(m => m.PriceAfter.GetValueOrDefault());
            return total;
        }
    }


}