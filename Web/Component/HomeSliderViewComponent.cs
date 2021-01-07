using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Component
{
    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMemoryCache _cache;
        public HomeSliderViewComponent(ISliderRepository sliderRepository, IMemoryCache cache)
        {
            _sliderRepository = sliderRepository;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_cache.TryGetValue("CACHE_MASTER_Slider", out List<Slider> cLstProd))
            {
                return await Task.Run(() => View("Index", cLstProd));
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
                Priority = CacheItemPriority.NeverRemove
            };
            var listslider = await _sliderRepository.All.ToListAsync();
            _cache.Set("CACHE_MASTER_Slider", listslider, options);
            return await Task.Run(() => View("Index", listslider));
        }
    }
}
