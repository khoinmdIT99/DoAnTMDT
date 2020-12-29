using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Products;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Component
{
    public class PopularViewComponent:ViewComponent
    {
        private readonly IProductRepository _productRepository;
        private IMemoryCache _memoryCache;
        public PopularViewComponent(IProductRepository productRepository, IMemoryCache memoryCache)
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
            var listfeature = productViewModels.Where(x => x.Actived == true && x.IsSpecial == true).OrderByDescending(x => x.Discount).ToList();
            var generatedStuff = new List<ProductViewModel>();
            int dem = 0;
            for (var i = 0; i < listfeature.Count(); i++)
            {
                if (dem == 2)
                {
                    break;
                }
                var rnd = new Random();
                int month = rnd.Next(0, listfeature.Count());
                generatedStuff.Add(listfeature[month]);
                dem++;
            }
            var categories = _memoryCache.GetOrCreate("PopularView", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(2);
                return generatedStuff;
            });
            return await Task.Run(() => View("Index", categories));
        }
    }
}
