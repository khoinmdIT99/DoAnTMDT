using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Task<List<CartProductViewModel>> task = new Task<List<CartProductViewModel>>(Excute);
            task.Start();
            var cart = await task;
            return View(cart);
        }
        public List<CartProductViewModel> Excute()
        {
            string session = HttpContext.Session.GetString("AddProducts");
            List<CartProductViewModel> cart = new List<CartProductViewModel>();
            if (session != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartProductViewModel>>(session);
            }
            return cart;
        }

    }
}
