using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.Products;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Controllers
{
   
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IServiceProvider _services;
        private readonly ICartRepository _cartRepository;

        public CartController(IProductRepository productRepository,IShoppingCartRepository shoppingCart, 
            IServiceProvider services, ICartRepository cartRepository)
        {
            this._productRepository = productRepository;
            this._shoppingCart = shoppingCart;
            this._services = services;
            this._cartRepository = cartRepository;
        }
        public string GetCart(IServiceProvider service)
        {

            try
            {
                HttpRequest cookie = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                if (cookie != null)
                {
                    string cartId = cookie.Cookies["cardId"] ?? Guid.NewGuid().ToString();
                    CookieOptions option = new CookieOptions {Expires = DateTime.Now.AddMonths(1)};
                    Response.Cookies.Append("cardId", cartId, option);
                    return cartId;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ViewResult ShoppingCart() {

            string cartId = GetCart(_services);
            var CartProductViewModels = new List<CartProductViewModel>();
            
            foreach (var item in _shoppingCart.GetCartProducts(cartId))
            {
                var cartProductViewModel = new CartProductViewModel() { 
                    Id = item.Id,
                    CartId = item.CartId,
                    Cart = _cartRepository.GetCartViewModel(item.CartId),
                    ProductId = item.ProductId,
                    Product = _productRepository.GetProductViewModelById(item.ProductId),
                    Price = item.Price,
                    PriceType = item.PriceType,
                    Quantity = item.Quantity,
                    Total = item.Total
                };
                CartProductViewModels.Add(cartProductViewModel);

            }
            var cart = new ShoppingCart()
            {
                Id = cartId,
                cartProducts = CartProductViewModels,
                Total = _shoppingCart.GetShoppingCartTotal(cartId)
            };
          
            var model = new ShoppingCartViewModel()
            {
                ShoppingCart = cart,
            };
            return View(model);
        }
        [HttpPost]
        public bool AddToShoppingCart(string id)
        {
            try
            {
                string cartId = GetCart(_services);
                var product = _productRepository.All.FirstOrDefault(d => d.Id == id);
                if (product != null)
                {      
                    _shoppingCart.AddToCart(product, cartId);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
           
        }

        public ActionResult AddToShoppingCartInDetail(ProductViewModel model, string quantity)
        {
            string cartId = GetCart(_services);
            var product = _productRepository.All.FirstOrDefault(d => d.Id == model.Id);
            if (product != null)
            {
                if (quantity != null)
                {
                    _shoppingCart.AddToCartWithQuantity(product, cartId, Convert.ToInt32(quantity));
                }

                return RedirectToAction("ShoppingCart");
            }

            return View("Detail", "Home");
        }

        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                _shoppingCart.RemoveFromCart(_productRepository.All.FirstOrDefault(d => d.Id == id), GetCart(_services));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public bool Update(string id , string quantity)
        {
            try
            {
                _shoppingCart.UpdateQuantityInCart(id, Convert.ToInt32(quantity),GetCart(_services));
                return true;
            }
            catch (Exception)
            {
                return false;
               
            }
            
        }
       
    }
}
