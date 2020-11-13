using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.Products;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
        public IActionResult ItemControl()
        {
            int basketCount = 0;
            string session = HttpContext.Session.GetString("AddProducts");
            List<CartProductViewModel> cart = new List<CartProductViewModel>();
            if (session != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartProductViewModel>>(session);
            }
            if (cart != null)
            {
                basketCount = cart.Count;
            }
            return Json(basketCount);
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
            var cartProductViewModels = new List<CartProductViewModel>();
            
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
                cartProductViewModels.Add(cartProductViewModel);

            }
            var cart = new ShoppingCart()
            {
                Id = cartId,
                cartProducts = cartProductViewModels,
                Total = _shoppingCart.GetShoppingCartTotal(cartId)
            };
          
            var model = new ShoppingCartViewModel()
            {
                ShoppingCart = cart,
            };
            return View(model);
        }
        public async Task AddToCartSession(string productId, int? quantity, string cartId)
        {

        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int? quantity)
        {
            string cartId = GetCart(_services);
            var product = await _productRepository.All.FirstOrDefaultAsync(d => d.Id == productId);
            if (product != null)
            {
                _shoppingCart.AddToCart(product, cartId);
            }
            var dto = new ProductViewModel();
            PropertyCopy.Copy(product, dto);
            var session = HttpContext.Session.Get<List<CartProductViewModel>>("AddProducts");
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;

                //Check exist with item product id
                if (session.Any(x => x.Id == productId))
                {
                    foreach (var item in session)
                    {
                        //Update quantity for product if match product id
                        if (item.Id == productId)
                        {
                            if (quantity != null) item.Quantity += (int)quantity;
                            if (product != null) item.Price = product.Price.GetValueOrDefault();
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    if (quantity != null)
                        if (product != null)
                            session.Add(new CartProductViewModel()
                            {
                                Id = product.Id,
                                Product = _productRepository.GetProductViewModelById(product.Id),
                                Price = product.Price.GetValueOrDefault(),
                                Quantity = quantity.Value
                            });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set("AddProducts", session);
                }
            }
            else
            {
                //Add new cart
                if (quantity != null)
                {
                    if (product != null)
                    {
                        var cart = new List<CartProductViewModel>
                        {
                            new CartProductViewModel()
                            {
                                Id = product.Id,Product = _productRepository.GetProductViewModelById(product.Id),
                                Price = product.Price.GetValueOrDefault(),
                                Quantity = quantity.Value
                            }
                        };
                        HttpContext.Session.Set("AddProducts", cart);
                    }
                }
            }
            return new OkObjectResult(productId);
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
