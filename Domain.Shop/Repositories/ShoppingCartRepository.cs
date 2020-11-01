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
        public List<CartProduct> cartProducts { get; set; }
        public ShoppingCartRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }
        public void AddToCart(Product product, string cartId)
        {
            var CartProduct = this.All.SingleOrDefault(
                s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);
            if (CartProduct == null )
            {
                Cart cart = DbContext.Carts.ToList().Where(c => c.Id == cartId).FirstOrDefault();
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        Id = cartId
                    };
                    DbContext.Carts.Add(cart);
                }
                CartProduct = new CartProduct()
                {
                    Id = Guid.NewGuid().ToString(),
                    Product = product,
                    Quantity = 1,
                    CartId = cartId
                };

                this.Add(CartProduct);
            }
            else
            {
                CartProduct.Quantity++;
            }
            this.Save();
        }
        public void RemoveFromCart(Product product, string CartId)
        {
            var cartProduct = this.All.SingleOrDefault(s => s.ProductId == product.Id && s.CartId == CartId && s.Bought == false);

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
        public List<CartProduct> GetCartProducts(string cardId)
        {
            var cart = this.All.Where(c => c.CartId == cardId && c.Bought == false)
                           .Include(s => s.Product).Include(s => s.Product.ProductType).Include(s => s.Product.ProductImages).Include(s => s.Product.ProductReviews)
                           .ToList();
            return cartProducts ?? cart;
        }

        public List<CartProduct> GetCartProductsBought(string cardId, string customerId)
        {
            var cart = this.All.Where(c => c.CartId == cardId && c.Bought == true && c.Cart.CustomerId == customerId)
                           .Include(s => s.Product).Include(s => s.Product.ProductType).Include(s => s.Product.ProductImages).Include(s => s.Product.ProductReviews)
                           .ToList();
            return cart;
        }
        public void ClearCart(string CartId)
        {
            var cartItems = this.All .Where(cart => cart.CartId == CartId && cart.Bought == false);
            foreach (var item in cartItems)
            {
                item.Bought = true;
            }
            this.Update(cartItems);
            this.Save();
        }

        public long GetShoppingCartTotal(string CartId)
        {
            var total = this.All.Where(c => c.CartId == CartId && c.Bought == false)
                .Select(c => c.Product.Price * c.Quantity).Sum();
            return total.Value;
        }
        public void UpdateQuantityInCart(string id, int quantity, string cartId)
        {
            try
            {

                var model = this.All.Where(s => s.Product.Id == id && s.CartId == cartId && s.Bought == false).FirstOrDefault();

                if (quantity == 0)
                {
                    this.Remove(model);
                    this.Save();
                }
                else
                {
                    if (model.Quantity != quantity)
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
            var CartProduct = this.All.SingleOrDefault(
                 s => s.ProductId == product.Id && s.CartId == cartId && s.Bought == false);
            if (CartProduct == null)
            {
                Cart cart = DbContext.Carts.ToList().Where(c => c.Id == cartId).FirstOrDefault();
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        Id = cartId
                    };
                    DbContext.Carts.Add(cart);
                }
                CartProduct = new CartProduct()
                {
                    Id = Guid.NewGuid().ToString(),
                    Product = product,
                    Quantity = quantity,
                    CartId = cartId
                };

                this.Add(CartProduct);
            }
            else
            {
                CartProduct.Quantity += quantity;
            }
            this.Save();
        }
    }
}
