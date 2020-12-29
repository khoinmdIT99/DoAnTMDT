using Domain.Shop.Dto.Cart;
using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class OrderController : BaseController
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IDictrictRepository _dictrictRepository;
        private readonly IProvinceRepository _provinceRepository;

        public OrderController(ICartRepository cartRepository, IAccountRepository accountRepository, 
            IProductRepository productRepository, IShoppingCartRepository shoppingCartRepository,IDictrictRepository 
            dictrictRepository, IProvinceRepository provinceRepository)
        {
            this._cartRepository = cartRepository;
            this._accountRepository = accountRepository;
            this._productRepository = productRepository;
            this._shoppingCartRepository = shoppingCartRepository;
            this._dictrictRepository = dictrictRepository;
            this._provinceRepository = provinceRepository;
        }
        public ActionResult Index()
        {
            List<CartViewModel> model = _cartRepository.GetCartViewModels().ToList();
            foreach (var item in model)
            {
                item.Customer = _accountRepository.GetCustomerViewModel(item.CustomerId);
                item.Customer.District = _dictrictRepository.GetDictrictViewModel(_accountRepository.GetCustomerViewModel(item.CustomerId).District).Name;
                item.Customer.Province = _provinceRepository.GetProvinceViewModel(_accountRepository.GetCustomerViewModel(item.CustomerId).Province).Name;
            }
            return View(model);
        }
        public IActionResult Print(string id)
        {
            var model = _cartRepository.GetCartViewModel(id);
            model.Customer = _accountRepository.GetCustomerViewModel(model.CustomerId);
            model.Customer.District = _dictrictRepository.GetDictrictViewModel(_accountRepository.GetCustomerViewModel(model.CustomerId).District).Name;
            model.Customer.Province = _provinceRepository.GetProvinceViewModel(_accountRepository.GetCustomerViewModel(model.CustomerId).Province).Name;
            List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();
            foreach (var cartProduct in _shoppingCartRepository.GetCartProductsBought(id, model.CustomerId))
            {
                var cartProductViewModel = new CartProductViewModel()
                {
                    Id = cartProduct.Id,
                    CartId = cartProduct.CartId,
                    Cart = _cartRepository.GetCartViewModel(cartProduct.CartId),
                    ProductId = cartProduct.ProductId,
                    Product = _productRepository.GetProductViewModelById(cartProduct.ProductId),
                    Price = cartProduct.Price,
                    PriceType = cartProduct.PriceType,
                    Quantity = cartProduct.Quantity,
                    Total = cartProduct.Total
                };
                cartProductViewModels.Add(cartProductViewModel);
                model.TotalPrice += cartProductViewModel.Total;
            }
            model.Products = cartProductViewModels;
            return View(model);
        }
        public IActionResult Detail(string id)
        {
            var model = _cartRepository.GetCartViewModel(id);
            model.Customer = _accountRepository.GetCustomerViewModel(model.CustomerId);
            model.Customer.District = _dictrictRepository.GetDictrictViewModel(_accountRepository.GetCustomerViewModel(model.CustomerId).District).Name;
            model.Customer.Province = _provinceRepository.GetProvinceViewModel(_accountRepository.GetCustomerViewModel(model.CustomerId).Province).Name;
            List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();
            foreach (var cartProduct in _shoppingCartRepository.GetCartProductsBought(id,model.CustomerId))
            {
                var cartProductViewModel = new CartProductViewModel()
                {
                    Id = cartProduct.Id,
                    CartId = cartProduct.CartId,
                    Cart = _cartRepository.GetCartViewModel(cartProduct.CartId),
                    ProductId = cartProduct.ProductId,
                    Product = _productRepository.GetProductViewModelById(cartProduct.ProductId),
                    Price = cartProduct.Price,
                    PriceType = cartProduct.PriceType,
                    Quantity = cartProduct.Quantity,
                    Total = cartProduct.Total
                };
                cartProductViewModels.Add(cartProductViewModel);
                model.TotalPrice += cartProductViewModel.Total;
            }
            model.Products = cartProductViewModels;
            return View(model);
        }
        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                var model = _cartRepository.GetCartViewModel(id);
                var cartProduct = _shoppingCartRepository.GetCartProductsBought(id,model.CustomerId);          
                Cart cart = _cartRepository.Get(id);
                _shoppingCartRepository.Delete(cartProduct);
                _cartRepository.Save();
                _cartRepository.Delete(cart);
                _cartRepository.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
