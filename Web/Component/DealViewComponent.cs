using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Products;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Component
{
    public class DealViewComponent : ViewComponent
    {
        private readonly IProductRepository _productRepository;
        private IMemoryCache _memoryCache;
        public DealViewComponent(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels();
            var productViewModels = list.ToList();
            foreach (var item in productViewModels)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            var listdealGroupBy = productViewModels.Where(x => x.Actived == true).GroupBy(user => user.ExtraDiscount);
            List<DealString> deaList = new List<DealString>();
            foreach (var group in listdealGroupBy)
            {
                if (group.Key.GetValueOrDefault() != 0)
                {
                    deaList.Add(new DealString()
                    {
                        href = "", ten = group.Key.GetValueOrDefault().ToString(CultureInfo.InvariantCulture)
                    });
                }
            }
            var categories = _memoryCache.GetOrCreate("DealView", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(2);
                return deaList;
            });
            categories[0].href = "#living-room";
            categories[1].href = "#kitchen";
            categories[2].href = "#work-palce";
            categories[3].href = "#wordrobe";


            ViewBag.Deal1 = _productRepository.GetProductViewModels().Where(x => x.Actived == true && Equals(x.ExtraDiscount, double.Parse(categories[0].ten))).ToList();
            ViewBag.Deal2 = _productRepository.GetProductViewModels().Where(x => x.Actived == true && Equals(x.ExtraDiscount, double.Parse(categories[1].ten))).ToList();
            ViewBag.Deal3 = _productRepository.GetProductViewModels().Where(x => x.Actived == true && Equals(x.ExtraDiscount, double.Parse(categories[2].ten))).ToList();
            ViewBag.Deal4 = _productRepository.GetProductViewModels().Where(x => x.Actived == true && Equals(x.ExtraDiscount, double.Parse(categories[3].ten))).ToList();

            return View("Index", categories.ToList());
        }
    }

    public class DealString
    {
        public string href;
        public string ten;
    }
}
