﻿using Domain.Shop.Dto.Cart;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class CartRepository : Repository<ShopDBContext, Cart>, ICartRepository
    {
        public CartRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
           
        }
        public void RemoveFromCart()
        {
            foreach (var i in All.ToList().Where(s => s.Total == 0))
            {
                this.Remove(i);
            }
            this.Save();
        }
        public CartViewModel GetCartViewModel(string id)
        {
           return this.All.Where(c => c.Id == id).Select(c => new CartViewModel()
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                TotalPrice = c.Totalprice,
                Total = c.Total,
               CreateAt = c.CreateAt,
               PaymentMethod = c.PaymentMethod,
               ShippingMethod = c.ShippingMethod,
               Status = c.Status,
               Comments = c.Comments
            }).FirstOrDefault();
        }
        public IEnumerable<CartViewModel> GetCartViewModels()
        {
            var model =  this.All.Where(c => c.CustomerId != null).Select(c => new CartViewModel()
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                TotalPrice = c.Totalprice,
                Total = c.Total,
                PaymentMethod = c.PaymentMethod,
                ShippingMethod = c.ShippingMethod,
                CreateAt = c.CreateAt,
                Status = c.Status,
                Comments = c.Comments
            }).ToList();
            return model;
        }
    }
}
