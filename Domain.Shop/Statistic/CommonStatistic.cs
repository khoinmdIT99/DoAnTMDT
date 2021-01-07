using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Domain.Shop.IRepositories;

namespace Domain.Shop.Statistic
{
    public class CommonStatistic
    {
        private readonly IProductRepository _iProductRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IImportRepository _importRepository;
        private readonly IImportDetailRepository _importDetailRepository;

        public CommonStatistic(IProductRepository iProductRepository, ICartRepository cartRepository, IShoppingCartRepository shoppingCartRepository, IImportRepository importRepository, IImportDetailRepository importDetailRepository)
        {
            _iProductRepository = iProductRepository;
            _cartRepository = cartRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _importRepository = importRepository;
            _importDetailRepository = importDetailRepository;
        }

        /// <summary> 
        /// Trả ra một danh sách các object, mỗi object chứa số hiệu tháng (int) , số hiệu năm (int) , tổng giá trị hóa đơn bán trong tháng đó.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="startMonth"></param>
        /// <param name="startYear"></param>
        /// <param name="endMonth"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        public List<MonthYear_SumSaleImportRevenue> GetTotalSaleImportInMonths(int? startMonth, int? startYear, int? endMonth, int? endYear)
        {
            DateTime startDate = (startMonth != null && startYear != null) ? new DateTime(startYear.Value, startMonth.Value, 1) : DateTime.Now.AddYears(-1);
            DateTime endDate = (endMonth != null && endYear != null) ? new DateTime(endYear.Value, endMonth.Value, DateTime.DaysInMonth(endYear.Value, endMonth.Value)) : DateTime.Now;

            DateTime iterator = startDate;
            List<DateTime> monthsToConsider = new List<DateTime>();
            while (true)
            {
                if (iterator > endDate) break;
                monthsToConsider.Add(iterator);
                iterator = iterator.AddMonths(1);
            }

            List<MonthYear_SumSaleImportRevenue> stats = new List<MonthYear_SumSaleImportRevenue>();
            foreach (DateTime month in monthsToConsider)
            {
                double sumSale = new SaleStatistic(_iProductRepository,_cartRepository,_shoppingCartRepository).GetTotalSaleValueCertainMonth(month.Month, month.Year);
                double sumImport = new ImportStatistic(_iProductRepository,_importRepository,_importDetailRepository).GetTotalImportValueCertainMonth(month.Month, month.Year);
                double difference = sumSale - sumImport;
                stats.Add(new MonthYear_SumSaleImportRevenue()
                {
                    Month = month.Month,
                    Year = month.Year,
                    TotalSale = (int)sumSale,
                    TotalImport = (int)sumImport,
                    TotalRevenue = (int)difference,
                });
            }

            return stats;
        }

        public double GetTotalRevenueAllTime()
        {
            return new SaleStatistic(_iProductRepository, _cartRepository, _shoppingCartRepository).GetTotalSaleValueAllTime() - new ImportStatistic(_iProductRepository, _importRepository, _importDetailRepository).GetTotalImportValueAllTime();
        }
    }
}