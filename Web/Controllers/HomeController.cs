using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Common.Security;
using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public IConfiguration Configuration { get; }
        public string Result { get; set; }
        public HomeController(IProductRepository productRepository, 
            IProductTagRepository productTagRepository, IServiceProvider serviceProvider,
            IProductReViewRepository productReViewRepository, IShoppingCartRepository shoppingCart, ICartRepository cartRepository, IConfiguration configuration)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._serviceProvider = serviceProvider;
            this._productReViewRepository = productReViewRepository;
            _shoppingCart = shoppingCart;
            _cartRepository = cartRepository;
            Configuration = configuration;
        }
        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [AllowAnonymous]
        public IActionResult Index()
        {
            string session = HttpContext.Session.GetString("AddProducts");
            ViewData["ReCaptchaKey"] = Configuration.GetSection("GoogleReCaptcha:key").Value;
            if (session == null)
            {
                _shoppingCart.RemoveFromCart();
                _cartRepository.RemoveFromCart();
                Response.Cookies.Delete("cardId", new CookieOptions()
                {
                    Secure = true,
                });
            }
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Age = HttpContext.Session.GetString(SessionId);
            return View(_productRepository.GetProductViewModels());
        }
        [AllowAnonymous]
        public IActionResult Index3()
        {
            return View(_productRepository.GetProductViewModels());
        }
        [AllowAnonymous]
        public IActionResult ProductList()
        {
            return View(_productRepository.GetProductViewModels());
        }

        [AllowAnonymous]
        public ActionResult GetProducts(string value)
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
        public IActionResult ProductDeltail(string id)
        {
            return View(_productRepository.GetProductViewModelById(id));
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
       
        public ActionResult Review(string id,string name, string review, string inlineRadioOptions)
        {
            try
            {
                HttpRequest cookie = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                if (cookie != null)
                {
                    string token  = cookie.Cookies[SecurityManager._securityToken];
                    string customerId = SecurityManager.getUserId(token);
                    ProductReview productReview = new ProductReview()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        Review = review,
                        ProductId = id,
                        Star = Convert.ToInt32(inlineRadioOptions),
                        CustomerId = customerId,
                        CreateAt = DateTime.UtcNow
                    };
                    _productReViewRepository.Add(productReview);
                }

                _productReViewRepository.Save();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
