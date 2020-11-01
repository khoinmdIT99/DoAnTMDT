using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Web.Models;

namespace Web.Component
{
    public class CartInLayoutViewComponent:ViewComponent
    {
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly ICartRepository _cartRepository;
        private readonly IServiceProvider _services;
        private readonly IProductRepository _productRepository;

        public CartInLayoutViewComponent(IShoppingCartRepository shoppingCart, ICartRepository cartRepository, IServiceProvider services, IProductRepository productRepository)
        {
            this._shoppingCart = shoppingCart;
            this._cartRepository = cartRepository;
            this._services = services;
            this._productRepository = productRepository;
        }
        public IViewComponentResult Invoke()
        {
            string cartId = GetCart(_services);
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

            var model = new ShoppingCartViewModel()
            {
                ShoppingCart = cart,
            };
            return View(model);
        }
        public string GetCart(IServiceProvider service)
        {

            try
            {
                HttpRequest cookie = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Request;
                return cookie?.Cookies["cardId"];
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
