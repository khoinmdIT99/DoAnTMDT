using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Web.Models;

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
            basketCount = cart?.Sum(item => item.Quantity) ?? 0;
            return Json(basketCount);
        }
        public IActionResult PriceControl()
        {
            long basketCount = 0;
            string session = HttpContext.Session.GetString("AddProducts");
            List<CartProductViewModel> cart = new List<CartProductViewModel>();
            if (session != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartProductViewModel>>(session);
            }
            basketCount = cart?.Sum(item => item.Price * item.Quantity) ?? 0;
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
                    CookieOptions option = new CookieOptions {Expires = DateTime.Now.AddHours(1)};
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
        [Route("giohang.html", Name = "ShoppingCart")]
        public ViewResult ShoppingCart() {
            string session = HttpContext.Session.GetString("AddProducts");

            if (session == null)
            {
                _shoppingCart.RemoveFromCart();
                _cartRepository.RemoveFromCart();
                Response.Cookies.Delete("cardId", new CookieOptions()
                {
                    Secure = true,
                });
                return View();
            }
            else
            {
                string cartId = GetCart(_services);
                var cartProductViewModels = new List<CartProductViewModel>();

                foreach (var item in _shoppingCart.GetCartProducts(cartId))
                {
                    var cartProductViewModel = new CartProductViewModel()
                    {
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
                    TotalPrice = _shoppingCart.GetShoppingCartTotalPrice(cartId),
                    Total = _shoppingCart.GetShoppingCartTotal(cartId)
                };

                var model = new ShoppingCartViewModel()
                {
                    ShoppingCart = cart,
                };
                return View(model);
            }
        }
        public async Task<IActionResult> ViewProduct(string productId)
        {
            var model = _productRepository.GetProductViewModelById(productId);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int? quantity)
        {
            string cartId = GetCart(_services);
            var returnMessage = new ReturnMessage();
            var product = await _productRepository.All.FirstOrDefaultAsync(d => d.Id == productId);
            if (product == null)
            {
                returnMessage.SetErrorMessage("Không tìm thấy trong danh sách sản phẩm.");
                return Json(returnMessage);
            }
            _shoppingCart.AddToCartSession(product, cartId,quantity.GetValueOrDefault());
            //var dto = new ProductViewModel();
            //PropertyCopy.Copy(product, dto);
            var session = HttpContext.Session.Get<List<CartProductViewModel>>("AddProducts");
            if (session != null)
            {
                //Check exist with item product id
                if (session.Any(x => x.Id == productId))
                {
                    foreach (var item in session)
                    {
                        //Update quantity for product if match product id
                        if (item.Id == productId)
                        {
                            //if (quantity != null) item.Quantity += (int)quantity;
                            //item.Price = product.Price.GetValueOrDefault();
                            //hasChanged = true;
                            if (quantity != null && quantity > 0)
                            {
                                item.Quantity = quantity.GetValueOrDefault();
                                returnMessage.setMessage("Sản phẩm đã có sẵn trong giỏ, bạn có muốn tăng thêm không?",item.Quantity);
                            }
                            HttpContext.Session.Set("AddProducts", session);
                            return Json(returnMessage);
                        }
                    }
                }
                else
                {
                    if (quantity != null)
                    {
                        session.Add(new CartProductViewModel()
                        {
                            Id = product.Id,
                            Product = _productRepository.GetProductViewModelById(product.Id),
                            Price = (long)product.PriceAfter.GetValueOrDefault(),
                            Quantity = quantity.Value
                        });
                    }
                    //hasChanged = true;
                    returnMessage.SetSuccessMessage("Sản phẩm đã được thêm thành công.");
                    HttpContext.Session.Set("AddProducts", session);
                    return Json(returnMessage);
                }

                //Update back to cart
                //if (hasChanged)
                //{
                //    HttpContext.Session.Set("AddProducts", session);
                //}
            }
            else
            {
                //Add new cart
                if (quantity != null)
                {
                    {
                        var cart = new List<CartProductViewModel>
                        {
                            new CartProductViewModel()
                            {
                                Id = product.Id,
                                Product = _productRepository.GetProductViewModelById(product.Id),
                                Price = (long)product.PriceAfter.GetValueOrDefault(),
                                Quantity = quantity.Value
                            }
                        };
                        HttpContext.Session.Set("AddProducts", cart);
                        returnMessage.SetSuccessMessage("Sản phẩm đã được thêm thành công.");
                        return Json(returnMessage);
                    }
                }
                else
                {
                    returnMessage.setMessageFirst("Kiểm tra số lượng");
                    return Json(returnMessage);
                }
            }
            return Json(returnMessage);
        }
        public IActionResult RemoveFromCart(string productId)
        {
            var cartId = GetCart(_services);
            var session = HttpContext.Session.Get<List<CartProductViewModel>>("AddProducts");
            if (session == null) return new EmptyResult();
            var hasChanged = false;
            foreach (var item in session.Where(item => item.Id == productId))
            {
                session.Remove(item);
                _shoppingCart.RemoveFromCartSession(_productRepository.All.FirstOrDefault(d => d.Id == item.Id), cartId);
                hasChanged = true;
                break;
            }
            if (hasChanged)
            {
                HttpContext.Session.Set("AddProducts", session);
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
                var session = HttpContext.Session.Get<List<CartProductViewModel>>("AddProducts");
                var cartProduct = session.FirstOrDefault(model => model.Id == id);
                if (cartProduct != null) 
                    cartProduct.Quantity--;
                HttpContext.Session.Set("AddProducts", session);
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
                var session = HttpContext.Session.Get<List<CartProductViewModel>>("AddProducts");
                var cartProduct = session.FirstOrDefault(model => model.Id == id);
                if (cartProduct != null)
                    cartProduct.Quantity = Convert.ToInt32(quantity);
                HttpContext.Session.Set("AddProducts", session);
                return true;
            }
            catch (Exception)
            {
                return false;
               
            }
            
        }
       
    }
}
