using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class InventorymanagementController : BaseController
    {
        private readonly IProductRepository _productRepository;

        public InventorymanagementController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult ByCategory()
        {
            var model = _productRepository.All.AsNoTracking().Include(x => x.Category).ToList()
                .GroupBy(p => p.Category)
                .Select(g => new ReportInfo
                {
                    Group = g.Key.CategoryName,
                    Count = g.Sum(p => p.BasketCount),
                    Sum = g.Sum(p => p.Price.GetValueOrDefault() * p.BasketCount),
                    Min = g.Min(p => p.Price.GetValueOrDefault()),
                    Max = g.Max(p => p.Price.GetValueOrDefault()),
                    Avg = g.Average(p => p.Price.GetValueOrDefault())
                });
            return View(model);
        }
        public IActionResult BySupplier()
        {
            var model = _productRepository.All.AsNoTracking().Include(x => x.Material).ToList()
                .GroupBy(p => p.Material)
                .Select(g => new ReportInfo
                {
                    Group = g.Key.MaterialName,
                    Count = g.Sum(p => p.BasketCount),
                    Sum = g.Sum(p => p.Price.GetValueOrDefault() * p.BasketCount),
                    Min = g.Min(p => p.Price.GetValueOrDefault()),
                    Max = g.Max(p => p.Price.GetValueOrDefault()),
                    Avg = g.Average(p => p.Price.GetValueOrDefault())
                });
            return View(model);
        }
    }
}