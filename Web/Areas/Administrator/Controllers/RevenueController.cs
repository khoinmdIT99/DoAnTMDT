using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Domain.Shop.Dto;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class RevenueController : BaseController
    {
        private readonly IShoppingCartRepository _iShoppingCartRepository;
        private readonly ICategoryRepository _categoryRepository;
        public RevenueController(IShoppingCartRepository iShoppingCartRepository, ICategoryRepository categoryRepository)
        {
            _iShoppingCartRepository = iShoppingCartRepository;
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult byProduct(DateTime? Min = null, DateTime? Max = null)
        {
            if (Min == null)
            {
                Min = DateTime.MinValue;
            }
            if (Max == null)
            {
                Max = DateTime.MaxValue;    
            }


            var model = _iShoppingCartRepository.All.AsNoTracking().Include(u => u.Product).ToList().Where(x=>x.Bought);

            if (!string.IsNullOrEmpty(Request.Query["status"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => x.Cart.Status == int.Parse(Request.Query["status"].ToString()));
            }
            if (!string.IsNullOrEmpty(Request.Query["datestart"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["datestart"].ToString(), StringComparison.Ordinal) >= 0);
                ViewBag.datestart = Request.Query["datestart"];
            }
            if (!string.IsNullOrEmpty(Request.Query["dateend"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["dateend"].ToString(), StringComparison.Ordinal) < 0);
                ViewBag.dateend = Request.Query["dateend"];
            }
            var model_rs = model.GroupBy(d => d.Product.ProductName).Select(g => new ReportInfo
            {
                Group = g.Key,
                Sum = g.Sum(d => d.Price * d.Quantity).GetValueOrDefault(),
                Count = g.Sum(d => d.Quantity),
                Min = g.Min(d => d.Price).GetValueOrDefault(),
                Max = g.Max(d => d.Price).GetValueOrDefault(),
                Avg = g.Average(d => d.Price).GetValueOrDefault()
            }).ToList();


            ViewBag.FlagSearch = true;
           
            return View(model_rs);
        }
        public IActionResult byCategory(DateTime? Min = null, DateTime? Max = null)
        {
            if (Min == null)
            {
                Min = DateTime.MinValue;
            }
            if (Max == null)
            {
                Max = DateTime.MaxValue;
            }
            var model = _iShoppingCartRepository.All.AsNoTracking().Include(u => u.Product.Category).Include(x => x.Cart).ToList().Where(x => x.Bought);
            if (!string.IsNullOrEmpty(Request.Query["status"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => x.Cart.Status == int.Parse(Request.Query["status"].ToString()));
            }
            if (!string.IsNullOrEmpty(Request.Query["datestart"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["datestart"].ToString(), StringComparison.Ordinal) >= 0);
                ViewBag.datestart = Request.Query["datestart"];
            }
            if (!string.IsNullOrEmpty(Request.Query["dateend"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["dateend"].ToString(), StringComparison.Ordinal) < 0);
                ViewBag.dateend = Request.Query["dateend"];
            }
            var model_rs = model.GroupBy(d => d.Product.Category)
                .Select(g => new ReportInfo
                {
                    Group = g.Key.ToString(),
                    Sum = g.Sum(x=>x.Cart.Totalprice),
                    Count = g.Count(),
                    Min = g.Min(x => x.Cart.Totalprice),
                    Max = g.Max(x => x.Cart.Totalprice),
                    Avg = g.Average(x => x.Cart.Totalprice)
                }).ToList();

            ViewBag.FlagSearch = true;
            return View(model_rs);
        }
        public IActionResult bySupplier(DateTime? Min = null, DateTime? Max = null)
        {
            if (Min == null)
            {
                Min = DateTime.MinValue;
            }
            if (Max == null)
            {
                Max = DateTime.MaxValue;
            }
            var model = _iShoppingCartRepository.All.AsNoTracking().Include(u => u.Product.Supplier).Include(u => u.Cart).ToList().Where(x => x.Bought);
            if (!string.IsNullOrEmpty(Request.Query["status"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => x.Cart.Status == int.Parse(Request.Query["status"].ToString()));
            }
            if (!string.IsNullOrEmpty(Request.Query["datestart"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["datestart"].ToString(), StringComparison.Ordinal) >= 0);
                ViewBag.datestart = Request.Query["datestart"];
            }
            if (!string.IsNullOrEmpty(Request.Query["dateend"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["dateend"].ToString(), StringComparison.Ordinal) < 0);
                ViewBag.dateend = Request.Query["dateend"];
            }

            var model_rs = model.GroupBy(d => d.Product.Supplier)
                .Select(g => new ReportInfo
                {
                    Group = g.Key.Name,
                    Sum = g.Sum(x => x.Cart.Totalprice),
                    Count = g.Count(),
                    Min = g.Min(x => x.Cart.Totalprice),
                    Max = g.Max(x => x.Cart.Totalprice),
                    Avg = g.Average(x => x.Cart.Totalprice)
                }).ToList();
            ViewBag.FlagSearch = true;
            return View(model_rs);
        }
        public IActionResult byCustomer(DateTime? Min = null, DateTime? Max = null)
        {
            if (Min == null)
            {
                Min = DateTime.MinValue;
            }
            if (Max == null)
            {
                Max = DateTime.MaxValue;
            }
            var model = _iShoppingCartRepository.All.AsNoTracking()
                .Include(x => x.Cart)
                .Include(x => x.Cart.Customer)
                .ToList().Where(x => x.Bought);
            if (!string.IsNullOrEmpty(Request.Query["status"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => x.Cart.Status == int.Parse(Request.Query["status"].ToString()));
            }
            if (!string.IsNullOrEmpty(Request.Query["datestart"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["datestart"].ToString(), StringComparison.Ordinal) >= 0);
                ViewBag.datestart = Request.Query["datestart"];
            }
            if (!string.IsNullOrEmpty(Request.Query["dateend"].ToString()))
            {
                // var a = "abc";
                model = model.Where(x => String.Compare(x.Cart.CreateAt?.ToString("yyyy/MM/dd"), Request.Query["dateend"].ToString(), StringComparison.Ordinal) < 0);
                ViewBag.dateend = Request.Query["dateend"];
            }

            var model_rs = model.GroupBy(d => d.Cart.Customer.FullName)
             .Select(g => new ReportInfo
             {
                 Group = g.Key,
                 Sum = g.Sum(d => d.Cart.Totalprice),
                 Count = g.Count(),
                 Min = g.Min(d => d.Cart.Totalprice),
                 Max = g.Max(d => d.Cart.Totalprice),
                 Avg = g.Average(d => d.Cart.Totalprice)
             }).ToList();
            
            ViewBag.FlagSearch = true;
            return View(model_rs);
        }
        public IActionResult byYear()
        {
            var model = _iShoppingCartRepository.All.AsNoTracking().Include(x => x.Cart).ToList()
                .GroupBy(d => d.Cart.CreateAt?.Year)
                .Select(g => new ReportInfo
                {
                    //iGroup = g.Key,
                    Group = "Year "+g.Key,
                    Sum = g.Sum(d=>d.Cart.Totalprice),
                    Count = g.Count(),
                    Min = g.Min(d => d.Cart.Totalprice),
                    Max = g.Max(d => d.Cart.Totalprice),
                    Avg = g.Average(d => d.Cart.Totalprice)
                })
                .OrderBy(i => i.Group).ToList();
            return View(model);
        }
        public IActionResult byMonth()
        {
            var model = _iShoppingCartRepository.All.AsNoTracking().Include(x => x.Cart).ToList()
                .Where(d => d.Cart.CreateAt?.Month == DateTime.Now.Month)
                .GroupBy(d => d.Cart.CreateAt.Value.Month)
                .Select(g => new ReportInfo
                {
                    Group="Month "+g.Key,
                    Sum = g.Sum(d => d.Cart.Totalprice),
                    Count = g.Count(),
                    Min = g.Min(d => d.Cart.Totalprice),
                    Max = g.Max(d => d.Cart.Totalprice),
                    Avg = g.Average(d => d.Cart.Totalprice)
                })
                .OrderBy(i => i.Group).ToList();
            return View(model);
        }
        public IActionResult byQuarter()
        {
            var model = _iShoppingCartRepository.All.AsNoTracking().Include(x => x.Cart).ToList()
                .GroupBy(d => (d.Cart.CreateAt?.Month - 1) / 3 + 1)
                .Select(g => new ReportInfo
                {
                    //iGroup = g.Key,
                    Group= "Quarter " + g.Key,
                    Sum = g.Sum(d => d.Cart.Totalprice),
                    Count = g.Count(),
                    Min = g.Min(d => d.Cart.Totalprice),
                    Max = g.Max(d => d.Cart.Totalprice),
                    Avg = g.Average(d => d.Cart.Totalprice)
                })
                .OrderBy(i => i.Group).ToList();
            return View(model);
        }
    }
}