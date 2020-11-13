//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Domain.Shop.Dto.CartProduct;
//using Domain.Shop.Dto.ShoppingCart;
//using Domain.Shop.IRepositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;

//namespace Web.Component
//{
//    public class HeaderCartViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
//    {
//        private readonly IProductRepository _productRepository;
//        private readonly IShoppingCartRepository _shoppingCart;
//        private readonly IServiceProvider _services;
//        private readonly ICartRepository _cartRepository;

//        public HeaderCartViewComponent(IProductRepository productRepository, IShoppingCartRepository shoppingCart, IServiceProvider services, ICartRepository cartRepository)
//        {
//            _productRepository = productRepository;
//            _shoppingCart = shoppingCart;
//            _services = services;
//            _cartRepository = cartRepository;
//        }
//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            ViewBag.ProductPhotos = await _iProductPhotoManager.All.ToListAsync();
//            Task<List<AddProductViewModel>> task = new Task<List<AddProductViewModel>>(Excute);
//            task.Start();
//            var cart = await task;
//            return View(cart);
//        }
//        public string GetCart(IServiceProvider service)
//        {

//            try
//            {
//                HttpRequest cookie = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
//                if (cookie != null)
//                {
//                    string cartId = cookie.Cookies["cardId"] ?? Guid.NewGuid().ToString();
//                    CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddMonths(1) };
//                    HttpContext.Response.Cookies.Append("cardId", cartId, option);
//                    return cartId;
//                }
//                else
//                {
//                    return "";
//                }
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }
//        public ShoppingCartViewModel ShoppingCart()
//        {
//            string cartId = GetCart(_services);
//            var cartProductViewModels = new List<CartProductViewModel>();

//            foreach (var item in _shoppingCart.GetCartProducts(cartId))
//            {
//                var cartProductViewModel = new CartProductViewModel()
//                {
//                    Id = item.Id,
//                    CartId = item.CartId,
//                    Cart = _cartRepository.GetCartViewModel(item.CartId),
//                    ProductId = item.ProductId,
//                    Product = _productRepository.GetProductViewModelById(item.ProductId),
//                    Price = item.Price,
//                    PriceType = item.PriceType,
//                    Quantity = item.Quantity,
//                    Total = item.Total
//                };
//                cartProductViewModels.Add(cartProductViewModel);

//            }
//            var cart = new ShoppingCart()
//            {
//                Id = cartId,
//                cartProducts = cartProductViewModels,
//                Total = _shoppingCart.GetShoppingCartTotal(cartId)
//            };

//            var model = new ShoppingCartViewModel()
//            {
//                ShoppingCart = cart,
//            };
//            return model;
//        }
//        public List<CartProductViewModel> Excute()
//        {
//            string session = HttpContext.Session.GetString("AddProducts");
//            List<CartProductViewModel> cart = new List<CartProductViewModel>();
//            if (session != null)
//            {
//                cart = JsonConvert.DeserializeObject<List<CartProductViewModel>>(session);
//            }
//            return cart;
//        }
//    }
//}
