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
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IMemoryCache _cache;
        public IConfiguration Configuration { get; }
        public string Result { get; set; }
        public HomeController(IProductRepository productRepository, 
            IProductTagRepository productTagRepository, IServiceProvider serviceProvider,
            IProductReViewRepository productReViewRepository, IShoppingCartRepository shoppingCart, ICartRepository cartRepository, IConfiguration configuration, ICategoryRepository categoryRepository, IBlogCategoryRepository blogCategoryRepository, IMemoryCache cache, IProductTypeRepository productTypeRepository, IMaterialRepository materialRepository)
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
            _productTypeRepository = productTypeRepository;
            _materialRepository = materialRepository;
        }
        [AllowAnonymous]
        public async Task<JsonResult> GetList(string name)
        {
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                return await Task.Run(() => Json(cLstProd.Where(x => x.Actived == true && x.ProductName.ToLower().StartsWith(name.ToLower())).Take(10).ToList()));
            }
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
                Priority = CacheItemPriority.NeverRemove
            };
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            _cache.Set("CACHE_MASTER_PRODUCT", list, options);
            var listactive = list.Where(x => x.Actived == true && x.ProductName.ToLower().StartsWith(name.ToLower())).Take(10).ToList();
            return await Task.Run(() => Json(listactive));
        }
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
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
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
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromHours(3),
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
        [Route("loaisanpham/{tenloai?}", Name = "Category-Detail-List")]
        [Route("loaisanpham/{tenloai?}/p{page:int}", Name = "Category-Detail-List-page")]
        [Route("loaisanpham/{tenloai?}/{chatlieu?}", Name = "Category-Detail-List-CL-main")]
        [Route("loaisanpham/{tenloai?}/{chatlieu?}/p{page:int}", Name = "Category-Detail-List-CL-main-page")]
        [Route("loaisanpham/{tenloai?}/{loaisanpham?}", Name = "Category-Detail-List-PT-main")]
        [Route("loaisanpham/{tenloai?}/{loaisanpham?}/p{page:int}", Name = "Category-Detail-List-PT-main-page")]
        [Route("loaisanpham/{tenloai?}/{tamgia?}", Name = "Category-Detail-List-TG-main")]
        [Route("loaisanpham/{tenloai?}/{tamgia?}/p{page:int}", Name = "Category-Detail-List-TG-main-page")]
        [Route("loaisanpham/{tenloai?}/{chatlieu?}/{loaisanpham?}", Name = "Category-Detail-List-sub")]
        [Route("loaisanpham/{tenloai?}/{chatlieu?}/{loaisanpham?}/p{page:int}", Name = "Category-Detail-List-sub-page")]
        public async Task<IActionResult> CategoryDetail(string tenloai,string chatlieu,string loaisanpham,string tamgia, int page)
        {
            var tenloaisanpham = await _categoryRepository.All.FirstOrDefaultAsync(x => x.Slug == (tenloai));
            var tenloaisanphamchitiet = tenloaisanpham.CategoryName;
            ViewBag.tenloai = tenloai;
            var listmaterial = await _materialRepository.All.ToListAsync();
            var listproducttype = await _productTypeRepository.All.ToListAsync();
            ViewBag.Dschatlieu = listmaterial;
            ViewBag.DsLoai = listproducttype;
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                var listactive = cLstProd.Where(x => x.Actived == true && x.CategoryName == tenloaisanphamchitiet).ToList();
                string url = $"/loaisanpham/{tenloai}/p{{0}}";
                if (loaisanpham == null && chatlieu == null)
                {
                    listactive = cLstProd.Where(x => x.Actived == true && x.CategoryName == tenloaisanphamchitiet).ToList();
                }
                if (tamgia != null && loaisanpham == null && chatlieu == null)
                {
                    ViewBag.chatlieu = null;
                    ViewBag.loaisanpham = null;
                    ViewBag.tamgia = tamgia;
                    url = $"/loaisanpham/{tenloai}/{tamgia}/p{{0}}";
                    if (tamgia.Contains("be20"))
                    {
                        listactive = listactive.Where(x =>
                            x.PriceAfter <= double.Parse("20000000")).ToList();
                    }
                    else if (tamgia.Contains("lon20"))
                    {
                        listactive = listactive.Where(x =>
                            x.PriceAfter > double.Parse("20000000")
                            && x.PriceAfter <= double.Parse("150000000")).ToList();
                    }
                    else if (tamgia.Contains("full"))
                    {
                        listactive = listactive.Where(x =>
                            x.PriceAfter > double.Parse("150000000")).ToList();
                    }
                }
                if (chatlieu != null && loaisanpham != null)
                {
                    ViewBag.chatlieu = chatlieu;
                    ViewBag.loaisanpham = loaisanpham;
                    url = $"/loaisanpham/{tenloai}/{chatlieu}/{loaisanpham}/p{{0}}";
                    var materialname = listmaterial.FirstOrDefault(x => x.SeoAlias == chatlieu)?.MaterialName;
                    var producttypename = listproducttype.FirstOrDefault(x => x.SeoAlias == loaisanpham)?.TypeName;
                    listactive = listactive.Where(x => 
                        x.MaterialName.Equals(materialname) 
                        && x.ProductTypeName.Equals(producttypename)).ToList();
                }
                // nếu có mid thì lọc theo mid
                if (chatlieu != null && loaisanpham == null)
                {
                    ViewBag.chatlieu = chatlieu;
                    var materialname = listmaterial.FirstOrDefault(x => x.SeoAlias == chatlieu)?.MaterialName;
                    url = $"/loaisanpham/{tenloai}/{chatlieu}/p{{0}}";
                    listactive = listactive.Where(x => 
                        x.MaterialName.Equals(materialname)).ToList();
                }

                // nếu có cid thì lọc theo cid
                if (loaisanpham != null && chatlieu == null)
                {
                    ViewBag.loaisanpham = loaisanpham;
                    var producttypename = listproducttype.FirstOrDefault(x => x.SeoAlias == loaisanpham)?.TypeName;
                    url = $"/loaisanpham/{tenloai}/{loaisanpham}/p{{0}}";
                    listactive = listactive.Where(x => 
                        x.ProductTypeName.Equals(producttypename)).ToList();
                }



                //sắp xếp theo ngày đăng
                    var data = listactive.OrderByDescending(x => x.CreateAt.GetValueOrDefault()).ToList();

                // dem so luong du lieu  phu hop 
                int totals = data.Count();

                // sử lý phân trang
                if (page <= 0)
                {
                    page = 1;
                }
                int pageSize = 3;
                int skip = (page - 1) * pageSize;

                var listData = data.Skip(skip).Take(pageSize);

                // TAO DŨ LIEU CHO VIEW
                ListDatasource datasource = new ListDatasource();
                datasource.Total = totals;
                datasource.Page = page;
                datasource.PageSize = pageSize;
                datasource.MaxPage = 6;
                datasource.Url = url;
                datasource.Data = listData.ToList();
                return await Task.Run(() => View(datasource));
            }
            else
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                    SlidingExpiration = TimeSpan.FromHours(3),
                    Priority = CacheItemPriority.NeverRemove
                };
                IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
                foreach (var item in list)
                {
                    item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
                }
                _cache.Set("CACHE_MASTER_PRODUCT", list, options);
                var listactive = list.Where(x => x.Actived == true && x.CategoryName == tenloaisanphamchitiet).ToList();
                string url = $"/loaisanpham/{tenloai}/p{{0}}";
                if (loaisanpham == null && chatlieu == null)
                {
                    listactive = list.Where(x => x.Actived == true && x.CategoryName == tenloaisanphamchitiet).ToList();
                }
                if (tamgia != null && loaisanpham == null && chatlieu == null)
                {
                    ViewBag.chatlieu = null;
                    ViewBag.loaisanpham = null;
                    ViewBag.tamgia = tamgia;
                    url = $"/loaisanpham/{tenloai}/{tamgia}/p{{0}}";
                    if (tamgia.Contains("be20"))
                    {
                        listactive = listactive.Where(x =>
                            x.PriceAfter <= double.Parse("20000000")).ToList();
                    }
                    else if (tamgia.Contains("lon20"))
                    {
                        listactive = listactive.Where(x =>
                            x.PriceAfter > double.Parse("20000000")
                            && x.PriceAfter <= double.Parse("150000000")).ToList();
                    }
                    else if (tamgia.Contains("full"))
                    {
                        listactive = listactive.Where(x =>
                            x.PriceAfter > double.Parse("150000000")).ToList();
                    }
                }
                if (chatlieu != null && loaisanpham != null)
                {
                    url = $"/loaisanpham/{tenloai}/{chatlieu}/{loaisanpham}/p{{0}}";
                    listactive = listactive.Where(x =>
                        x.MaterialName.ToLower().Equals(chatlieu.ToLower())
                        && x.ProductTypeName.ToLower().Equals(loaisanpham.ToLower())).ToList();
                }
                // nếu có mid thì lọc theo mid
                if (chatlieu != null && loaisanpham == null)
                {
                    url = $"/loaisanpham/{tenloai}/{chatlieu}/p{{0}}";
                    listactive = listactive.Where(x =>
                        x.MaterialName.ToLower().Equals(chatlieu.ToLower())).ToList();
                }

                // nếu có cid thì lọc theo cid
                if (loaisanpham != null && chatlieu == null)
                {
                    url = $"/loaisanpham/{tenloai}/{loaisanpham}/p{{0}}";
                    listactive = listactive.Where(x =>
                        x.ProductTypeName.ToLower().Equals(loaisanpham.ToLower())).ToList();
                }

                //sắp xếp theo ngày đăng
                var data = listactive.OrderByDescending(x => x.CreateAt.GetValueOrDefault()).ToList();

                // dem so luong du lieu  phu hop 
                int totals = data.Count;

                // sử lý phân trang
                if (page <= 0)
                {
                    page = 1;
                }
                int pageSize = 3;
                int skip = (page - 1) * pageSize;

                var listData = data.Skip(skip).Take(pageSize);

                // TAO DŨ LIEU CHO VIEW
                ListDatasource datasource = new ListDatasource();
                datasource.Total = totals;
                datasource.Page = page;
                datasource.PageSize = pageSize;
                datasource.MaxPage = 6;
                datasource.Url = url;
                datasource.Data = listData.ToList();
                return await Task.Run(() => View(datasource));
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> ProductList()
        {
            return await Task.Run(() => View(_productRepository.GetProductViewModels()));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Check()
        {
            IEnumerable<BlogCategoryViewModel> blogCategories = _blogCategoryRepository.GetBlogCategoryViewModels();
            blogCategories = BlogCategoryViewModel.GetTreeBlogCategoryViewModels(blogCategories);
            return await Task.Run(() => Json(blogCategories));
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts(string value)
        {
            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByOrder(Convert.ToInt32(value)).ToList();
                return await Task.Run(() => Json(new { data = productList }));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> GetProductCategory(string value)
        {
            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByOrder(Convert.ToInt32(value)).ToList();
                return await Task.Run(() => Json(new { data = productList }));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search_Index(string keyword = null)
        {
            if (keyword != null)
            {
                ViewBag.keyword = keyword;
            }
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                var listsearch = cLstProd.Where(x => keyword != null && (x.Actived == true
                                                                         && x.ProductName.ToLower()
                                                                             .Contains(keyword?.ToLower()))).ToList();
                ViewBag.listsearch = listsearch.Count;
                return await Task.Run(()
                    => View(listsearch));
            }
            else
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                    SlidingExpiration = TimeSpan.FromHours(3),
                    Priority = CacheItemPriority.NeverRemove
                };
                IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
                foreach (var item in list)
                {
                    item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
                }
                _cache.Set("CACHE_MASTER_PRODUCT", list, options);
                var listactive = list.Where(x => keyword != null && (x.Actived == true
                                                                     && x.ProductName.ToLower()
                                                                         .Contains(keyword?.ToLower()))).ToList();
                ViewBag.listsearch = listactive.Count;
                return await Task.Run(()
                    => View(listactive));
            }
            
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetSearchingData(string SearchBy = null, string SearchValue = null)
        {
            if (SearchValue == null || SearchBy == null)
            {
                return Json(new List<ProductViewModel>());
            }
            else
            {
                if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
                {
                    if (SearchBy == "TrongTamGia")
                    {
                        try
                        {
                            return await Task.Run(()
                                => Json(cLstProd
                                    .Where(x => x.Actived == true
                                                && (x.PriceAfter.GetValueOrDefault() > 0 &&
                                                    x.PriceAfter.GetValueOrDefault() <= double.Parse(SearchValue))).ToList()));
                        }
                        catch (Exception e)
                        {
                            return await Task.Run(()
                                => Json(cLstProd
                                    .Where(x => x.Actived == true)
                                    .ToList()));
                        }
                    }
                    else
                    {
                        return await Task.Run(()
                            => Json(cLstProd.Where(x => x.Actived == true
                                                        && x.ProductName.ToLower()
                                                            .Contains(SearchValue.ToLower())).ToList()));
                    }
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                    SlidingExpiration = TimeSpan.FromHours(3),
                    Priority = CacheItemPriority.NeverRemove
                };
                IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
                foreach (var item in list)
                {
                    item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
                }
                _cache.Set("CACHE_MASTER_PRODUCT", list, options);
                if (SearchBy == "TrongTamGia")
                {
                    try
                    {
                        return await Task.Run(()
                            => Json(list
                                .Where(x => x.Actived == true
                                            && (x.PriceAfter.GetValueOrDefault() > 0 &&
                                                x.PriceAfter.GetValueOrDefault() <= double.Parse(SearchValue))).ToList()));
                    }
                    catch (Exception e)
                    {
                        return await Task.Run(()
                            => Json(list
                                .Where(x => x.Actived == true)
                                .ToList()));
                    }
                }
                else
                {
                    return await Task.Run(()
                        => Json(list.Where(x => x.Actived == true
                                                    && x.ProductName.ToLower()
                                                        .Contains(SearchValue.ToLower())).ToList()));
                }
            }
        }
        //[HttpGet]
        //public IActionResult GetPatient(int id = 0)
        //{
        //    if (id == 0)
        //    {
        //        return View(new List<ProductViewModel>());
        //    }
        //    else
        //    {
        //        IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();


        //        var serialized = JsonConvert.SerializeObject(list);

        //        //This will make your content-type as JSON.
        //        return Content(serialized, "application/json");
        //    }
        //}
        [AllowAnonymous]
        [Route("commingsoon.html", Name = "Commingsoon")]
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
        public async Task<IActionResult> ProductDeltail(string slug)
        {
            var sp = _productRepository.GetProductViewModelBySlug(slug);
            ViewBag.ProductTagDetail = _productTagRepository.GetProductTagViewModelsByProductId(sp.Id).Select(s => s.TagName).ToList();
            ViewBag.RelateProduct = _productRepository.GetProductViewModelsByOrder(5).ToList();
            if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<ProductViewModel> cLstProd))
            {
                var listactive = cLstProd.Where(x => x.Actived == true && x.IsNew == true).Take(10).ToList();
                ViewBag.NewProduct = listactive;
            }
            else
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(3),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                    SlidingExpiration = TimeSpan.FromHours(3),
                    Priority = CacheItemPriority.NeverRemove
                };
                IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels().ToList();
                foreach (var item in list)
                {
                    item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
                }
                _cache.Set("CACHE_MASTER_PRODUCT", list, options);
                var listactive = list.Where(x => x.Actived == true && x.IsNew == true).Take(10).ToList();
                ViewBag.NewProduct = listactive;
            }
            return await Task.Run(()
                => View(sp));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string categoryName)
        {
            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByCategory(categoryName).ToList();
                return await Task.Run(()
                    => Json(new { data = productList }));
            }
            catch (Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> SearchByPrice(string min, string max)
        {

            try
            {
                List<ProductViewModel> productList = _productRepository.GetProductViewModelsByPrice(Convert.ToInt32(min), Convert.ToInt32(max)).ToList();
                return await Task.Run(()
                    => Json(new { data = productList }));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> SortProducts(string value)
        {
            try
            {
                return await Task.Run(()
                    => Json(new { data = _productRepository.SortProductViewModels(value)}));
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
        public async Task<string> Review(string id,string name, string review, string inlineRadioOptions)
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
                        Star = Convert.ToInt32(inlineRadioOptions),
                        CreateAt = DateTime.UtcNow
                    };
                    await _productReViewRepository.AddAsync(productReview);
                    await _productReViewRepository.SaveAsync();
                    thongbao = "Cảm ơn bạn đã đánh giá sản phẩm";
                }
                return await Task.Run(()
                    => thongbao);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
