using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class StatisticController : BaseController
    {
        private readonly ICartRepository _iCartRepository;
        private readonly ICustomerRepository _iCustomerRepository;
        private readonly IProductRepository _iProductRepository;
        private readonly ICategoryRepository _iCategoryRepository;

        public StatisticController(ICartRepository iCartRepository, ICustomerRepository iCustomerRepository, IProductRepository iProductRepository, ICategoryRepository iCategoryRepository)
        {
            _iCartRepository = iCartRepository;
            _iCustomerRepository = iCustomerRepository;
            _iProductRepository = iProductRepository;
            _iCategoryRepository = iCategoryRepository;
        }

        public ActionResult Index()
        {

            ViewBag.Orders = _iCartRepository.All.ToList().Where(x => x.CreateAt?.ToString("yyyy-MM-dd")
            == DateTime.Now.ToString("yyyy-MM-dd") && x.Status == 0).ToList().Count;
            ViewBag.Users = _iCustomerRepository.All.ToList().Count;
            ViewBag.Product = _iProductRepository.All.ToList().Count;
            ViewBag.Category = _iCategoryRepository.All.ToList().Count;
            try
            {
                var list_str = GetListDate(12);


                var LeftJoin_ = (from m in list_str
                                 join o in _iCartRepository.All.Where(x => x.Status == 0).ToList()
                                 on m equals o.CreateAt?.ToString("yyyy-MM") into joinedDateOrder
                                 from o in joinedDateOrder.DefaultIfEmpty()
                                 select new { Date = m, Count = o != null ? o.Totalprice : 0, Status = o != null ? o.Status : -1 })
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


        //public ActionResult GetProductImportHistory_JSON(int productID)
        //{
        //    //return Json(Statistic.ImportStatistic.GetProductImportHistory(productID));

        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.ImportStatistic.GetProductImportHistory(productID),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetProductAverageCostHistory_JSON(int productID)
        //{
        //    //return Json(Statistic.ImportStatistic.GetProductAverageCostHistory(productID));

        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.ImportStatistic.GetProductAverageCostHistory(productID),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetProductAvailabilityCheckHistory_JSON(int productID)
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.ProductStatistic.GetProductAvailabilityHistory(productID),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetProductSaleHistory_JSON(int? productID, DateTime? dateFrom, DateTime? dateTo)
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.SaleStatistic.GetProductSaleHistory(productID, dateFrom, dateTo),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetBestSellingProducts_JSON(DateTime? dateFrom, DateTime? dateTo)
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.SaleStatistic.GetBestSellingProducts(dateFrom, dateTo),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetLeastSoldProducts_JSON(DateTime? dateFrom, DateTime? dateTo)
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.SaleStatistic.GetLeastSoldProducts(dateFrom, dateTo),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetTotalSaleImportInMonths_JSON(int? startMonth, int? startYear, int? endMonth, int? endYear)
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.CommonStatistic.GetTotalSaleImportInMonths(this, startMonth, startYear, endMonth, endYear),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetTotalRevenue_JSON()
        //{
        //    //GetTotalRevenueAllTime
        //    string converted = JsonConvert.SerializeObject(
        //        Statistic.CommonStatistic.GetTotalRevenueAllTime(this),
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}

        //public ActionResult GetCommonInfo_JSON()
        //{
        //    string converted = JsonConvert.SerializeObject(
        //        new
        //        {
        //            //Tổng lợi nhuận
        //            totalRevenue = Statistic.CommonStatistic.GetTotalRevenueAllTime(this),
        //            //Tổng số đơn hàng
        //            totalSaleBillCount = Statistic.SaleStatistic.CountSaleBillAllTime(this),
        //            //Tổng số người dùng
        //            totalCustomer = Statistic.CustomerStatistic.CountCustomer(this),
        //            //Tổng giá trị hàng tồn kho
        //            totalProductValue = Statistic.ProductStatistic.GetCurrentTotalProductValue(this)
        //        },
        //        Formatting.None,
        //        new IsoDateTimeConverter()
        //        {
        //            DateTimeFormat = "yyyy-MM-dd"
        //        });
        //    return Content(converted, "application/json");
        //}
    }
}
