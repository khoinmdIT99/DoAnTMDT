using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Common.Security;
using Domain.Shop.Dto.BlogCategories;
using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Component;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        private readonly IProductRepository _productRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IProductReViewRepository _productReViewRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IMemoryCache _cache;
        public IConfiguration Configuration { get; }
        public string Result { get; set; }
        public HomeController(IProductRepository productRepository, 
            IProductTagRepository productTagRepository, IServiceProvider serviceProvider,
            IProductReViewRepository productReViewRepository, IShoppingCartRepository shoppingCart, ICartRepository cartRepository, IConfiguration configuration, ICategoryRepository categoryRepository, IBlogCategoryRepository blogCategoryRepository, IMemoryCache cache)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._serviceProvider = serviceProvider;
            this._productReViewRepository = productReViewRepository;
            _shoppingCart = shoppingCart;
            _cartRepository = cartRepository;
            Configuration = configuration;
            _categoryRepository = categoryRepository;
            _blogCategoryRepository = blogCategoryRepository;
            _cache = cache;
        }
        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [AllowAnonymous]
        [Route("", Name = "Index")]
        [Route("trangchu.html", Name = "TrangChu")]
        public IActionResult Index(string thongbao = null)
        {
            string session = HttpContext.Session.GetString("AddProducts");
            
            if (session == null)
            {
                _shoppingCart.RemoveFromCart();
                _cartRepository.RemoveFromCart();
                Response.Cookies.Delete("cardId", new CookieOptions()
                {
                    Secure = true,
                });
            }
            if (thongbao != null)
            {
                ViewBag.ThongBao = thongbao;
            }
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Age = HttpContext.Session.GetString(SessionId);
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> listproductcache))
            {
                return View(listproductcache);
            }
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                SlidingExpiration = TimeSpan.FromSeconds(60),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            return View(list);
        }
        [AllowAnonymous]
        [Route("sanpham.html", Name = "SanPham")]
        public async Task<IActionResult> Index3()
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                return await Task.Run(() => View(cLstProd.Where(x => x.Actived == true).ToList()));
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                SlidingExpiration = TimeSpan.FromSeconds(60),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            var listactive = list.Where(x => x.Actived == true).ToList();
            return await Task.Run(() => View(listactive));
        }
        [AllowAnonymous]
        public IActionResult ProductList()
        {
            return View(_productRepository.GetProductViewModels());
        }
        [AllowAnonymous]
        public IActionResult Check()
        {
            IEnumerable<BlogCategoryViewModel> blogCategories = _blogCategoryRepository.GetBlogCategoryViewModels();
            blogCategories = BlogCategoryViewModel.GetTreeBlogCategoryViewModels(blogCategories);
            return Json(blogCategories);
        }
        [AllowAnonymous]
        public IActionResult GetProducts(string value)
        {
            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByOrder(Convert.ToInt32(value)).ToList();
                return Json(new { data = productList });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        [Route("sanpham/chitiet/{slug}", Name = "SanPhamChiTiet")]
        public IActionResult ProductDeltail(string slug)
        {
            return View(_productRepository.GetProductViewModelBySlug(slug));
        }
        [AllowAnonymous]
        public IActionResult ABC(string slug)
        {
            return Content(slug);
        }
        [AllowAnonymous]
        public ActionResult Filter(string categoryName)
        {
            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByCategory(categoryName).ToList();
                return Json(new { data = productList });
            }
            catch (Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        public ActionResult SearchByPrice(string min, string max)
        {

            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByPrice(Convert.ToInt32(min), Convert.ToInt32(max)).ToList();
                return Json(new { data = productList });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [AllowAnonymous]
        public JsonResult SortProducts(string value)
        {
            try
            {
                return Json(new { data = _productRepository.SortProductViewModels(value)});
            }
            catch (Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        public ActionResult Detail(string id)
        {
            ViewBag.ProductTagDetail = _productTagRepository.GetProductTagViewModelsByProductId(id).Select(s => s.TagName).ToList();
            ViewBag.RelateProduct = _productRepository.GetProductViewModelsByOrder(5).ToList();
            var model = _productRepository.GetProductViewModelById(id);
            return View(model);
        }
        [AllowAnonymous]
        public async Task<string> Review(string id,string name, string review)
        {
            string thongbao = "";
            try
            {
                if (HttpContext.Session.GetString(SessionId) == null)
                {
                    thongbao = "Vui lòng đăng nhập để review";
                }
                else
                {
                    ProductReview productReview = new ProductReview()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        Review = review,
                        ProductId = id,
                        CustomerId = HttpContext.Session.GetString(SessionId),
                        CreateAt = DateTime.UtcNow
                    };
                    await _productReViewRepository.AddAsync(productReview);
                    await _productReViewRepository.SaveAsync();
                    thongbao = "Cảm ơn bạn đã đánh giá sản phẩm";
                }
                return thongbao;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
