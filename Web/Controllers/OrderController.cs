using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common.Security;
using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.Order;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCart;
        readonly IServiceProvider _services;
        private readonly IAccountRepository _accountRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProvinceRepository _iProvinceRepository;
        private readonly IDictrictRepository _iDictrictRepository;
        const string SessionId = "_Id";

        public OrderController(IProductRepository productRepository, IShoppingCartRepository shoppingCart, IServiceProvider services, 
            IAccountRepository accountRepository, ICartRepository cartRepository, IDictrictRepository iDictrictRepository, IProvinceRepository iProvinceRepository)
        {
            this._productRepository = productRepository;
            this._shoppingCart = shoppingCart;
            this._services = services;
            this._accountRepository = accountRepository;
            this._cartRepository = cartRepository;
            this._iDictrictRepository = iDictrictRepository;
            this._iProvinceRepository = iProvinceRepository;
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult TabThongTinTaiKhoan()
        {
            HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
            if (cookie != null)
            {
                string cartId = cookie.Cookies["cardId"];
                List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

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
                    Total = _shoppingCart.GetShoppingCartTotal(cartId)
                };
                //get customer
                string customerId = SecurityManager.getUserId(cookie.Cookies[SecurityManager._securityToken]);

                var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                if (HttpContext.Session.GetString(SessionId) == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (customer == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var model = new OrderViewModel()
                {
                    CustomerId = customer.Id,
                    FullName = customer.FirstName + " " + customer.LastName,
                    Address = customer.Address,
                    District = customer.District,
                    Province = customer.Province,
                    PhoneNo = customer.PhoneNo,
                    Email = customer.Email,
                    ShoppingCart = cart
                };
                ViewBag.province = model.Province == null ? new SelectList(_iProvinceRepository.All, "Id", "Name") : new SelectList(_iProvinceRepository.All, "Id", "Name", model.Province);
                ViewBag.district = model.District == null ? new SelectList(_iDictrictRepository.All, "Id", "Name") : new SelectList(_iDictrictRepository.All, "Id", "Name", model.District);
                return PartialView("_PartialThongTinTaiKhoan",model);
            }
            return PartialView("_PartialThongTinTaiKhoan");
        }
        [AllowAnonymous]
        public async Task<IActionResult> CheckLogin()
        {
            var customer =
                await _accountRepository.All.FirstOrDefaultAsync(x => x.Id == HttpContext.Session.GetString(SessionId));
            if(customer != null)
                return Json(new { success = true });
            return Json(new { success = false, message = "Vui lòng đăng nhập để mua hàng" });
        }
        [AllowAnonymous]
        public IActionResult HomeOrder()
        {
            try
            {
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                if (cookie != null)
                {
                    string cartId = cookie.Cookies["cardId"];
                    List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

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
                        Total = _shoppingCart.GetShoppingCartTotal(cartId)
                    };
                    //get customer
                    string customerId = SecurityManager.getUserId(cookie.Cookies[SecurityManager._securityToken]);

                    var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                    if (HttpContext.Session.GetString(SessionId) == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (customer == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new OrderViewModel()
                    {
                        CustomerId = customer.Id,
                        FullName = customer.FirstName +" "+customer.LastName,
                        Address = customer.Address,
                        District = customer.District,
                        Province = customer.Province,
                        PhoneNo = customer.PhoneNo,
                        Email = customer.Email,
                        ShoppingCart = cart
                    };
                    ViewBag.PayTypes = GetPayList();
                    ViewBag.ShipTypes = GetShipList();
                    ViewBag.province = model.Province == null ? new SelectList(_iProvinceRepository.All, "Id", "Name") : new SelectList(_iProvinceRepository.All, "Id", "Name", model.Province);
                    ViewBag.district = model.District == null ? new SelectList(_iDictrictRepository.All, "Id", "Name") : new SelectList(_iDictrictRepository.All, "Id", "Name",model.District);
                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddCart(OrderViewModel model)
        {
            try
            {
                ViewBag.PayTypes = GetPayList();
                ViewBag.ShipTypes = GetShipList();
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                var customer = _accountRepository.GetCustomerViewModel(HttpContext.Session.GetString(SessionId));
                if (cookie != null)
                {
                    string cartId = cookie.Cookies["cardId"];
                    Cart cart = _cartRepository.Get(cartId);
                    cart.CreateAt = DateTime.UtcNow;
                    cart.CustomerId = customer.Id;
                    cart.Total += _shoppingCart.GetShoppingCartTotal(cartId);
                    cart.Totalprice += _shoppingCart.GetShoppingCartTotalPrice(cartId);
                    cart.PaymentMethod = model.PaymentMethod;
                    cart.ShippingMethod = model.ShippingMethod;
                    cart.Comments = model.Comment;
                    _cartRepository.UpdateAsync(cart);
                }
                await _cartRepository.SaveAsync();
                if (cookie != null) _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                HttpContext.Session.Remove("AddProducts");
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/[controller]/CapNhatNguoiDung")]
        public async Task<IActionResult> CapNhatNguoiDung([FromBody] Customer data)
        {
            var message ="";
            if (data is null)
            {
                message = "Vui lòng điền đầy đủ thông tin bên trên";
                return Ok(message);
            }
            var nd = await _accountRepository.All.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (nd == null)
            {
                message = "Người dùng không tồn tại";
                return Ok(message);
            }
            nd.FullName = data.FullName;
            nd.Email = data.Email;
            nd.LastUpdateAt = DateTime.Now;
            nd.Address = data.Address;
            nd.District = data.District;
            nd.PhoneNo = data.PhoneNo;
            nd.Province = data.Province;
            try
            {
                _accountRepository.UpdateAsync(nd);
                await _accountRepository.SaveAsync();
                message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Ok(message);
        }
        private List<SelectListModel> GetPayList()
        {
            return FunctionHelper.TypePay();
        }
        private List<SelectListModel> GetShipList()
        {
            return FunctionHelper.TypeShip();
        }
    }
}
