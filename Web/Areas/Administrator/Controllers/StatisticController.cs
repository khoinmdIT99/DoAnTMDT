using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.IRepositories;
using Domain.Shop.Statistic;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class StatisticController : BaseController
    {
        private readonly ICustomerRepository _iCustomerRepository;
        private readonly IProductRepository _iProductRepository;
        private readonly ICategoryRepository _iCategoryRepository;
        private readonly ICartRepository _iCartRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IImportRepository _importRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly ICartRepository _cartRepository;
        public StatisticController(ICartRepository iCartRepository, ICustomerRepository iCustomerRepository, IProductRepository iProductRepository, ICategoryRepository iCategoryRepository, IShoppingCartRepository shoppingCartRepository, IImportRepository importRepository, IImportDetailRepository importDetailRepository, ICartRepository cartRepository)
        {
            _iCartRepository = iCartRepository;
            _iCustomerRepository = iCustomerRepository;
            _iProductRepository = iProductRepository;
            _iCategoryRepository = iCategoryRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _importRepository = importRepository;
            _importDetailRepository = importDetailRepository;
            _cartRepository = cartRepository;
        }
        [HttpGet]
        public FileResult Download(string fileName)
        {
            string filepath = System.IO.Path.Combine("D:\\", fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/pdf", fileName);
        }
        public List<double?> TotalMoneyOrderAndImportWithMonth()
        {
            List<double?> lsttotalmoney = new List<double?>();
            List<double?> lsttotal = new List<double?>();
            long? total = 1;
            for (int i = 1; i <= 12; i++)
            {
                var totalmoney = _iCartRepository.All.ToList().Where(
                    x => x.CreateAt?.Month == i &&
                         x.CreateAt?.Year == DateTime.Today.Year &&
                         x.Status == "Đã xử lý").Sum(x => x.Totalprice);
                var totalmoneyImport = _importRepository.All.ToList().Where(
                    x => DateTime.Parse(x.DateCreated).Month == i &&
                         DateTime.Parse(x.DateCreated).Year == DateTime.Today.Year).Sum(x => x.TotalValue);
                lsttotal.Add((double)totalmoneyImport);
                lsttotalmoney.Add((double)totalmoney);
            }
            List<double?> lstpercenttotalmoney = new List<double?>();
            for (int i = 0; i < lsttotalmoney.Count; i++)
            {
                if ((double)lsttotal[i] == 0)
                {
                    lstpercenttotalmoney.Add(0);
                }
                else
                {
                    lstpercenttotalmoney.Add((lsttotalmoney[i] / lsttotal[i]) * 100);
                }
            }
            return lstpercenttotalmoney;
        }

        public async Task<IActionResult> Check()
        {
            DateTime startDate = new DateTime(2021, 1, 1) ;
            DateTime endDate = new DateTime(2021, 6, DateTime.DaysInMonth(2021, 6)) ;

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
                double sumSale = new SaleStatistic(_iProductRepository, _cartRepository, _shoppingCartRepository).GetTotalSaleValueCertainMonth(month.Month, month.Year);
                double sumImport = new ImportStatistic(_iProductRepository, _importRepository, _importDetailRepository).GetTotalImportValueCertainMonth(month.Month, month.Year);
                double difference = sumSale - sumImport;
                stats.Add(new MonthYear_SumSaleImportRevenue()
                {
                    Month = month.Month,
                    Year = month.Year,
                    TotalSale = sumSale,
                    TotalImport = sumImport,
                    TotalRevenue = difference,
                });
            }

            return Json(stats);
        }
        public async Task<IActionResult> Index()
        {

            ViewBag.Orders = _iCartRepository.All.ToList().Where(x => x.CreateAt?.ToString("yyyy-MM-dd")
            == DateTime.Now.ToString("yyyy-MM-dd") && x.Status == "Đã xử lý").ToList().Count;
            ViewBag.Users = await _iCustomerRepository.All.CountAsync();
            ViewBag.Product = await _iProductRepository.All.CountAsync();
            ViewBag.Category = await _iCategoryRepository.All.CountAsync();
            //Gán số lượng sản phẩm đã bán
            ViewBag.SalesedQuantity = await _iProductRepository.All.SumAsync(x =>x.BuyCount);
            //Gán số lượng sản phẩm còn lại
            ViewBag.RemainingQuantity =await _iProductRepository.All.SumAsync(x => x.BasketCount);
            ViewBag.TotalMoneyWithMonth = JsonConvert.SerializeObject(TotalMoneyOrderAndImportWithMonth());
            try
            {
                var list_str = GetListDate(12);


                var LeftJoin_ = (from m in list_str
                                 join o in _iCartRepository.All.Where(x => x.Status == "Đã xử lý").ToList()
                                 on m equals o.CreateAt?.ToString("yyyy-MM") into joinedDateOrder
                                 from o in joinedDateOrder.DefaultIfEmpty()
                                 select new { Date = m, Count = o != null ? o.Totalprice : 0, Status = o != null ? o.Status : "Sai" })
                                 .GroupBy(x => x.Date).Select(g => new DataChart { Label = g.Key, Value = g.Sum(x => x.Count).ToString() });


                var list_data = LeftJoin_.ToList();
                ViewBag.DataChart = list_data;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("<b>Lỗi {0}</b>.", e.Message + e.InnerException.Message), false);
            }
            return View();
        }
        public List<string> GetListDate(int count)
        {
            var list = new List<String>();
            // count = 12;
            var now = DateTime.Now;
            for (var i = 1; i <= count; i++)
            {
                var str = now.AddMonths(-(i)).ToString("yyyy-MM");
                list.Add(str);
            }
            return list;
        }

        public IActionResult GetProductImportHistory_JSON(string productID)
        {
            //return Json(Statistic.ImportStatistic.GetProductImportHistory(productID));

            string converted = JsonConvert.SerializeObject(
                new ImportStatistic(_iProductRepository, _importRepository, _importDetailRepository).GetProductImportHistory(productID),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        public IActionResult GetProductAverageCostHistory_JSON(string productID)
        {
            string converted = JsonConvert.SerializeObject(
                new ImportStatistic(_iProductRepository,_importRepository,_importDetailRepository).GetProductAverageCostHistory(productID),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        //public IActionResult GetProductAvailabilityCheckHistory_JSON(int productID)
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        new ProductStatistic().GetProductAvailabilityHistory(productID),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        public IActionResult GetProductSaleHistory_JSON(string productID, DateTime? dateFrom, DateTime? dateTo)
        {
            string converted = JsonConvert.SerializeObject(
                new SaleStatistic(_iProductRepository,_iCartRepository,_shoppingCartRepository).GetProductSaleHistory(productID, dateFrom, dateTo),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        public IActionResult GetBestSellingProducts_JSON(DateTime? dateFrom, DateTime? dateTo)
        {
            string converted = JsonConvert.SerializeObject(
                new SaleStatistic(_iProductRepository,_iCartRepository,_shoppingCartRepository).GetBestSellingProducts(dateFrom, dateTo),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        public IActionResult GetLeastSoldProducts_JSON(DateTime? dateFrom, DateTime? dateTo)
        {
            string converted = JsonConvert.SerializeObject(
                new SaleStatistic(_iProductRepository, _iCartRepository, _shoppingCartRepository).GetLeastSoldProducts(dateFrom, dateTo),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        public IActionResult GetTotalSaleImportInMonths_JSON(int? startMonth, int? startYear, int? endMonth, int? endYear)
        {
            string converted = JsonConvert.SerializeObject(
                new CommonStatistic(_iProductRepository, _iCartRepository, _shoppingCartRepository,_importRepository,_importDetailRepository).GetTotalSaleImportInMonths(startMonth, startYear, endMonth, endYear),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        public IActionResult GetTotalRevenue_JSON()
        {
            //GetTotalRevenueAllTime
            string converted = JsonConvert.SerializeObject(
                new CommonStatistic(_iProductRepository, _iCartRepository, _shoppingCartRepository, _importRepository, _importDetailRepository).GetTotalRevenueAllTime(),
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }

        public IActionResult GetCommonInfo_JSON()
        {
            double a = new CommonStatistic(_iProductRepository, _iCartRepository, _shoppingCartRepository,
                _importRepository, _importDetailRepository).GetTotalRevenueAllTime();
            if (a == 0)
            {
                a = 0;
            }
            string converted = JsonConvert.SerializeObject(
                new
                {
                    //Tổng lợi nhuận
                    totalRevenue =  a,
                    //Tổng số đơn hàng
                    totalSaleBillCount = new SaleStatistic(_iProductRepository, _iCartRepository, _shoppingCartRepository).CountSaleBillAllTime(),
                    //Tổng số người dùng
                    totalCustomer = new CustomerStatistic(_iCustomerRepository).CountCustomer(),
                    //Tổng giá trị hàng tồn kho
                    totalProductValue = new ProductStatistic(_iProductRepository).GetCurrentTotalProductValue()
                },
                Formatting.None,
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd"
                });
            return Content(converted, "application/json");
        }
    }
}
