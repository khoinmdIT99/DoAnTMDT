using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Domain.Shop.IRepositories;

namespace Domain.Shop.Statistic
{
    public class ImportStatistic
    {
        private readonly IProductRepository _iProductRepository;
        private readonly IImportRepository _importRepository;
        private readonly IImportDetailRepository _importDetailRepository;

        public ImportStatistic(IProductRepository iProductRepository, IImportRepository importRepository, IImportDetailRepository importDetailRepository)
        {
            _iProductRepository = iProductRepository;
            _importRepository = importRepository;
            _importDetailRepository = importDetailRepository;
        }

        public  List<Date_Amount_MoneyInt> GetProductImportHistory(string productID)
        {
            List<Date_Amount_MoneyInt> histories = (from importBillDetail in _importDetailRepository.All
                                                    join importBill in _importRepository.All
                                                    on importBillDetail.IdImport equals importBill.IdImport
                                                    where importBillDetail.IdProduct == productID
                                                    select new Date_Amount_MoneyInt()
                                                    {
                                                        Date = importBill.DateCreated,
                                                        Amount = importBillDetail.Amount.GetValueOrDefault(),
                                                        Money = importBillDetail.Price.GetValueOrDefault(),
                                                    }).OrderBy(h => h.Date).ToList();
            return histories;
        }

        public  List<Date_Amount_MoneyDouble> GetProductAverageCostHistory(string productID)
        {
            List<Date_Amount_MoneyInt> importHistories = GetProductImportHistory(productID); //Sorted by date !.
            List<Date_Amount_MoneyDouble> averCostHistories = new List<Date_Amount_MoneyDouble>();


            double averageCost = 0;
            int availability = 0;
            for (int i = 0; i < importHistories.Count; i++)
            {
                averageCost = (averageCost * availability + (double)importHistories[i].Money * importHistories[i].Amount) / (availability + importHistories[i].Amount);
                availability += importHistories[i].Amount;

                averCostHistories.Add(new Date_Amount_MoneyDouble()
                {
                    Date = importHistories[i].Date,
                    Money = averageCost,
                });
            }

            return averCostHistories;
        }

        /// <summary>
        /// Trả ra tổng giá trị hóa đơn bán hàng trong 1 tháng xác định.
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public decimal GetTotalImportValueCertainMonth(int month, int year)
        {
            DateTime startTime = new DateTime(year, month, 1);
            DateTime endTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            
            var importBillQuery = (
                from importBill in _importRepository.All.ToList()
                where DateTime.ParseExact(importBill.DateCreated, "dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) >= startTime && DateTime.ParseExact(importBill.DateCreated, "dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) <= endTime
                select importBill
                );

            decimal sum = importBillQuery.ToList().Sum(m => m.TotalValue);

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
        public List<MonthYear_Amount> GetTotalImportValueInMonths(int startMonth, int startYear, int endMonth, int endYear)
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
                    Amount = (long)GetTotalImportValueCertainMonth(month.Month, month.Year),
                });
            }

            return stats;
        }

        public long GetTotalImportValueAllTime()
        {   
            var importBillQuery = (
                from importBill in _importRepository.All
                select importBill
                );

            var sum = importBillQuery.ToList().Sum(m => m.TotalValue);

            return (long)sum;
        }
    }




}