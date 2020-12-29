using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Products;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Component
{
    public class BestSellerViewComponent : ViewComponent
    {
        private readonly IProductRepository _productRepository;
        private IMemoryCache _memoryCache;
        public BestSellerViewComponent(IProductRepository productRepository, IMemoryCache memoryCache)
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
            var listseller = productViewModels.Where(x => x.Actived == true && x.Discount > 25).OrderByDescending(x => x.Discount);
            var categories = _memoryCache.GetOrCreate("SellerView", entry => {
                entry.SlidingExpiration = TimeSpan.FromHours(2);
                return listseller;
            });
            return await Task.Run(() => View("Index", categories));
        }
    }
}
