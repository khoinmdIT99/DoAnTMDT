using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IShoppingCartRepository : IRepository<CartProduct>
    {
        void AddToCart(Product product, string cartId);
        void AddToCartSession(Product product, string cartId,int quantity);
        void AddToCartWithQuantity(Product product, string cartId, int quantity);
        void RemoveFromCart(Product product, string CartId);
        void RemoveFromCartSession(Product product, string CartId);
        void RemoveFromCart();
        List<CartProduct> GetCartProducts(string cardId);
        int GetShoppingCartTotal(string CartId);
        long GetShoppingCartTotalPrice(string CartId);
        void ClearCart(string CartId);
        void UpdateQuantityInCart(string id, int quantity, string cartId);
        List<CartProduct> GetCartProductsBought(string cardId, string customerId);
    }
}
