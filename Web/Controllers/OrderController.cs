using System;
using System.Collections.Generic;
using Domain.Common.Security;
using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.Order;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OrderController(IProductRepository productRepository, IShoppingCartRepository shoppingCart, IServiceProvider services, 
            IAccountRepository accountRepository, ICartRepository cartRepository)
        {
            this._productRepository = productRepository;
            this._shoppingCart = shoppingCart;
            this._services = services;
            this._accountRepository = accountRepository;
            this._cartRepository = cartRepository;
        }
       
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

                    var customer = _accountRepository.GetCustomerViewModel(customerId);
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
        [HttpPost]
        public IActionResult AddCart(OrderViewModel model)
        {
            try
            {
                HttpRequest cookie = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                if (cookie != null)
                {
                    string cartId = cookie.Cookies["cardId"];
                    Cart cart = _cartRepository.Get(cartId);
                    cart.CustomerId = model.CustomerId;
                    cart.CreateAt = DateTime.UtcNow;
                    cart.Total += _shoppingCart.GetShoppingCartTotal(cartId);
                    _cartRepository.Update(cart);
                }

                _cartRepository.Save();
                if (cookie != null) _shoppingCart.ClearCart(cookie.Cookies["cardId"]);
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
