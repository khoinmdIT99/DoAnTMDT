using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.ShoppingCart;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class ShoppingCartRepository : Repository<ShopDBContext, CartProduct>, IShoppingCartRepository
    {
        public List<CartProduct> CartProducts { get; set; }
        public ShoppingCartRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }
        public void AddToCartSession(Product product, string cartId,int quantity)
        {
            var cartProduct = this.All.FirstOrDefault(
                s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);
            if (cartProduct == null)
            {
                Cart cart = DbContext.Carts.ToList().FirstOrDefault(c => c.Id == cartId);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        Id = cartId
                    };
                    DbContext.Carts.Add(cart);
                }
                cartProduct = new CartProduct()
                {
                    Id = Guid.NewGuid().ToString(),
                    Product = product,
                    Quantity = quantity,
                    Price = product.Price,
                    CartId = cartId
                };

                this.Add(cartProduct);

            }
            else
            {
                cartProduct.Quantity = quantity;
                this.Update(cartProduct);
            }
            this.Save();
        }
        public void AddToCart(Product product, string cartId)
        {
            var cartProduct = this.All.SingleOrDefault(
                s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);
            if (cartProduct == null )
            {
                Cart cart = DbContext.Carts.ToList().FirstOrDefault(c => c.Id == cartId);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        Id = cartId
                    };
                    DbContext.Carts.Add(cart);
                }
                cartProduct = new CartProduct()
                {
                    Id = Guid.NewGuid().ToString(),
                    Product = product,
                    Quantity = 1,
                    CartId = cartId
                };

                this.Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity++;
            }

            this.Save();
        }
        public void RemoveFromCart(Product product, string cartId)
        {
            var cartProduct = this.All.SingleOrDefault(s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);

            if (cartProduct != null)
            {
                if (cartProduct.Quantity > 1)
                {
                    cartProduct.Quantity--;
                }
                else
                {
                    this.Remove(cartProduct);
                }
            }
            this.Save();
        }
        public void RemoveFromCartSession(Product product, string cartId)
        {
            var cartProduct = this.All.SingleOrDefault(s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);

            if (cartProduct != null)
            { 
                this.Remove(cartProduct);
            }
            this.Save();
        }
        public void RemoveFromCart()
        {
            foreach (var i in All.ToList().Where(s => s.Bought == false))
            {
                this.Remove(i);
            }
            this.Save();
        }
        public List<CartProduct> GetCartProducts(string cardId)
        {
            var cart = this.All.Where(c => c.CartId == cardId && c.Bought == false)
                           .Include(s => s.Product).Include(s => s.Product.ProductType).Include(s => s.Product.ProductImages).Include(s => s.Product.ProductReviews)
                           .ToList();
            return CartProducts ?? cart;
        }

        public List<CartProduct> GetCartProductsBought(string cardId, string customerId)
        {
            var cart = this.All.Where(c => c.CartId == cardId && c.Bought == true && c.Cart.CustomerId == customerId)
                           .Include(s => s.Product).Include(s => s.Product.ProductType).Include(s => s.Product.ProductImages).Include(s => s.Product.ProductReviews)
                           .ToList();
            return cart;
        }
        public void ClearCart(string cartId)
        {
            var cartItems = this.All .Where(cart => cart.CartId == cartId && cart.Bought == false);
            foreach (var item in cartItems)
            {
                item.Bought = true;
            }
            this.Update(cartItems);
            this.Save();
        }

        public int GetShoppingCartTotal(string cartId)
        {
            var total = this.All.Where(c => c.CartId == cartId && c.Bought == false)
                .Select(c => c.Quantity).Sum();
            return total;
        }
        public long GetShoppingCartTotalPrice(string cartId)
        {
            var total = this.All.Where(c => c.CartId == cartId && c.Bought == false)
                .Select(c => c.Product.Price * c.Quantity).Sum();
            return total.GetValueOrDefault();
        }
        public void UpdateQuantityInCart(string id, int quantity, string cartId)
        {
            try
            {

                var model = this.All.FirstOrDefault(s => s.Product.Id == id && s.CartId == cartId && s.Bought == false);

                if (quantity == 0)
                {
                    this.Remove(model);
                    this.Save();
                }
                else
                {
                    if (model != null && model.Quantity != quantity)
                    {
                        model.Quantity = quantity;
                        this.Update(model);
                    }
                    this.Save();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddToCartWithQuantity(Product product, string cartId, int quantity)
        {
            var cartProduct = this.All.SingleOrDefault(
                 s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);
            if (cartProduct == null)
            {
                Cart cart = DbContext.Carts.ToList().FirstOrDefault(c => c.Id == cartId);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        Id = cartId
                    };
                    DbContext.Carts.Add(cart);
                }
                cartProduct = new CartProduct()
                {
                    Id = Guid.NewGuid().ToString(),
                    Product = product,
                    Quantity = quantity,
                    CartId = cartId
                };

                Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity += quantity;
            }
            this.Save();
        }
    }
}
