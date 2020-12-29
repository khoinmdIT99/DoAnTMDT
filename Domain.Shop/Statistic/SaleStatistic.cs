using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shop.IRepositories;

namespace Domain.Shop.Statistic
{
    public class SaleStatistic
    {
        private readonly IProductRepository _iProductRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IShoppingCartRepository _shoppingCart;
        public SaleStatistic(IProductRepository iProductRepository, ICartRepository cartRepository, IShoppingCartRepository shoppingCart)
        {
            _iProductRepository = iProductRepository;
            _cartRepository = cartRepository;
            _shoppingCart = shoppingCart;
        }
        public List<SaleHistory> GetProductSaleHistory(string productID, DateTime? dateFrom, DateTime? dateTo)
        {
            var product_query = (
                from product in _iProductRepository.All
                select product
                );

            var saleBill_query = (
                from saleBill in _cartRepository.All
                select saleBill
                );

            var saleBillDetail_query = (
                from saleBillDetail in _shoppingCart.All
                select saleBillDetail
                );

            if (productID != null)
            {
                saleBillDetail_query = (from x in saleBillDetail_query where x.ProductId == productID select x);
                product_query = (from x in product_query where x.Id == productID select x);
            }

            if (dateFrom != null)
            {
                saleBill_query = (from x in saleBill_query where x.CreateAt >= dateFrom select x);
            }

            if (dateTo != null)
            {
                saleBill_query = (from x in saleBill_query where x.CreateAt <= dateTo select x);
            }

            List<SaleHistory> saleHistoryList = (
                from saleBill in saleBill_query
                join saleBillDetail in saleBillDetail_query on saleBill.Id equals saleBillDetail.CartId
                join product in product_query on saleBillDetail.ProductId equals product.Id
                select new SaleHistory()
                {
                    ProductID = saleBillDetail.ProductId,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    Date = saleBill.CreateAt.GetValueOrDefault(),
                    Amount = saleBillDetail.Quantity,
                    Price = saleBillDetail.Price.GetValueOrDefault()
                }
                ).OrderBy(m => m.Date).ThenBy(m => m.ProductID).ToList();

            return saleHistoryList;
        }

        public List<Product_Amount> GetBestSellingProducts(DateTime? dateFrom, DateTime? dateTo)
        {
            var saleBillDetails = (
                from saleBillDetail in _shoppingCart.All
                select saleBillDetail
                );

            if (dateFrom != null)
            {
                saleBillDetails = (from saleBillDetail in saleBillDetails
                                   join saleBill in _cartRepository.All on saleBillDetail.CartId equals saleBill.Id
                                   where saleBill.CreateAt >= dateFrom
                                   select saleBillDetail
                                   );
            }

            if (dateTo != null)
            {
                saleBillDetails = (from saleBillDetail in saleBillDetails
                                   join saleBill in _cartRepository.All on saleBillDetail.CartId equals saleBill.Id
                                   where saleBill.CreateAt <= dateTo
                                   select saleBillDetail
                                   );
            }

            var productID_and_sumSaleAmount_query = (from saleBillDetail in saleBillDetails
                                                     group saleBillDetail by saleBillDetail.ProductId into saleBillDetail_groupBy_productID
                                                     select new
                                                     {
                                                         ProductID = saleBillDetail_groupBy_productID.Key,
                                                         Sum = saleBillDetail_groupBy_productID.Sum(m => m.Quantity),
                                                     }
                );

            var saleHistory = (
                from productID_and_sumSaleAmount in productID_and_sumSaleAmount_query
                join product in _iProductRepository.All on productID_and_sumSaleAmount.ProductID equals product.Id
                select new Product_Amount()
                {
                    ProductID = productID_and_sumSaleAmount.ProductID,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    SumAmount = productID_and_sumSaleAmount.Sum,
                }
            ).OrderByDescending(m => m.SumAmount);

            return saleHistory.ToList();
        }

        public List<Product_Amount> GetLeastSoldProducts(DateTime? dateFrom, DateTime? dateTo)
        {
            var saleBillDetails = (
                from saleBillDetail in _shoppingCart.All
                select saleBillDetail
                );

            if (dateFrom != null)
            {
                saleBillDetails = (from saleBillDetail in saleBillDetails
                                   join saleBill in _cartRepository.All on saleBillDetail.CartId equals saleBill.Id
                                   where saleBill.CreateAt >= dateFrom
                                   select saleBillDetail
                                   );
            }

            if (dateTo != null)
            {
                saleBillDetails = (from saleBillDetail in saleBillDetails
                                   join saleBill in _cartRepository.All on saleBillDetail.CartId equals saleBill.Id
                                   where saleBill.CreateAt <= dateTo
                                   select saleBillDetail
                                   );
            }

            var productID_and_sumSaleAmount_query = (from saleBillDetail in saleBillDetails
                                                     group saleBillDetail by saleBillDetail.ProductId into saleBillDetail_groupBy_productID
                                                     select new
                                                     {
                                                         ProductID = saleBillDetail_groupBy_productID.Key,
                                                         Sum = saleBillDetail_groupBy_productID.Sum(m => m.Quantity),
                                                     }
                );

            var xxxx = (
                from product in _iProductRepository.All
                from productID_and_sumSaleAmount in productID_and_sumSaleAmount_query
                     .Where(productID_and_sumSaleAmount => productID_and_sumSaleAmount.ProductID == product.Id)
                     .DefaultIfEmpty()
                select new Product_Amount()
                {
                    ProductID = product.Id,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    SumAmount = productID_and_sumSaleAmount == null ? 0 : productID_and_sumSaleAmount.Sum,
                }
            ).OrderBy(m => m.SumAmount);

            return xxxx.ToList();
        }

        /// <summary>
        /// Trả ra tổng giá trị hóa đơn bán hàng trong 1 tháng xác định.
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public long GetTotalSaleValueCertainMonth(int month, int year)
        {
            DateTime startTime = new DateTime(year, month, 1);
            DateTime endTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var saleBillQuery = (
                from saleBill in _cartRepository.All
                where saleBill.CreateAt >= startTime && saleBill.CreateAt <= endTime
                select saleBill
                );

            long sum = saleBillQuery.ToList().Sum(m => m.Totalprice);
            

            return sum;
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
        public List<MonthYear_Amount> GetTotalSaleValueInMonths(int startMonth, int startYear, int endMonth, int endYear)
        {
            DateTime startDate = new DateTime(startYear, startMonth, 1);
            DateTime endDate = new DateTime(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth));

            DateTime iterator = startDate;
            List<DateTime> monthsToConsider = new List<DateTime>();
            while (true)
            {
                if (iterator > endDate) break;
                monthsToConsider.Add(iterator);
                iterator = iterator.AddMonths(1);
            }

            List<MonthYear_Amount> stats = new List<MonthYear_Amount>();
            foreach (DateTime month in monthsToConsider)
            {
                stats.Add(new MonthYear_Amount()
                {
                    Month = month.Month,
                    Year = month.Year,
                    Amount = GetTotalSaleValueCertainMonth(month.Month, month.Year),
                });
            }

            return stats;
        }


        public long GetTotalSaleValueAllTime()
        {
            var saleBillQuery = (
                from saleBill in _cartRepository.All
                select saleBill
                );

            long sum = saleBillQuery.ToList().Sum(m => m.Totalprice);

            return sum;
        }

        public int CountSaleBillAllTime()
        {
            var saleBillQuery = (
                from saleBill in _cartRepository.All
                select saleBill
                );

            int count = saleBillQuery.Count();

            return count;
        }
    }


}