using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Shop.Application;
using Infrastructure.Common;
using Domain.Shop.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.Shop.Repositories
{
    public class ProductRepository : Repository<ShopDBContext, Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork<ShopDBContext> unitOfWork, IMemoryCache cache) : base(unitOfWork)
        {
        }
        public Product GetProductById(string id)
        {
            return All.FirstOrDefault(m => m.Id == id);
        }
        public ProductViewModel GetProductViewModelById(string id)
        {
            var model =this.All.Where(m => m.Id == id).Select(m => new ProductViewModel
            {
                Id = m.Id,
                BasketCount = m.BasketCount,
                BuyCount = m.BuyCount,
                ProductCode = m.ProductCode,
                ProductName = m.ProductName,
                Slug = m.Slug,
                Description = m.Description,
                ProductTypeId = m.ProductTypeId,
                ProductTypeName = m.ProductType.TypeName,
                MaterialId = m.MaterialId,
                MaterialName = m.Material.MaterialName,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.CategoryName,
                PriceType = m.PriceType.ToString(),
                Price = m.Price,
                DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                Discount = m.Discount,
                ExtraDiscount = m.ExtraDiscount,
                IsNew = m.IsNew,
                IsFeatured = m.IsFeatured,
                IsSpecial = m.IsSpecial,
                Actived = m.Actived,
                Views = m.Views,
                CreateAt = m.CreateAt.GetValueOrDefault(),
                PriceAfter = Math.Round((double) ((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
            }).FirstOrDefault();
            return model;
        }
        public ProductViewModel GetProductViewModelBySlug(string slug)
        {
            var model = this.All.Where(m => Equals(m.Slug.ToLower(),slug.ToLower())).Include(m => m.ProductImages).Select(m => new ProductViewModel
            {
                Id = m.Id,
                BasketCount = m.BasketCount,
                BuyCount = m.BuyCount,
                ProductCode = m.ProductCode,
                ProductName = m.ProductName,
                Slug = m.Slug,
                Description = m.Description,
                ProductTypeId = m.ProductTypeId,
                ProductTypeName = m.ProductType.TypeName,
                MaterialId = m.MaterialId,
                MaterialName = m.Material.MaterialName,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.CategoryName,
                PriceType = m.PriceType.ToString(),
                Price = m.Price,
                DisplayImages = m.ProductImages.Where(x => x.ProductId == m.Id).OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                Discount = m.Discount,
                ExtraDiscount = m.ExtraDiscount,
                IsNew = m.IsNew,
                IsFeatured = m.IsFeatured,
                IsSpecial = m.IsSpecial,
                Actived = m.Actived,
                Views = m.Views,
                CreateAt = m.CreateAt.GetValueOrDefault(),
                PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
            }).FirstOrDefault();
            return model;
        }
        public IEnumerable<ProductViewModel> GetProductViewModels()
        {
            IEnumerable<ProductViewModel> model = null;

            try
            {
                model = this.All
                    .Include(x => x.Category)
                    .Include(x => x.Material)
                    .Include(x => x.ProductType)
                    .Select(m => new ProductViewModel
                {
                    Id = m.Id,
                    BasketCount = m.BasketCount,
                    BuyCount = m.BuyCount,
                    ProductCode = m.ProductCode,
                    ProductName = m.ProductName,
                    Slug = m.Slug,
                    Description = m.Description,
                    ProductTypeId = m.ProductTypeId,
                    ProductTypeName = m.ProductType.TypeName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.Material.MaterialName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    PriceType = m.PriceType.ToString(),
                    Price = m.Price,
                    Star  = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                    DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                    Discount = m.Discount,
                    ExtraDiscount = m.ExtraDiscount,
                    IsNew = m.IsNew,
                    IsFeatured = m.IsFeatured,
                    IsSpecial = m.IsSpecial,
                    Actived = m.Actived,
                    Views = m.Views,
                    CreateAt = m.CreateAt.GetValueOrDefault(),
                    PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
                }).ToList().OrderBy(p => p.ProductName).ToList();
            }
            catch (Exception)
            {
                model = this.All.Select(m => new ProductViewModel
                {
                    Id = m.Id,
                    BasketCount = m.BasketCount,
                    BuyCount = m.BuyCount,
                    ProductCode = m.ProductCode,
                    ProductName = m.ProductName,
                    Slug = m.Slug,
                    Description = m.Description,
                    ProductTypeId = m.ProductTypeId,
                    ProductTypeName = m.ProductType.TypeName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.Material.MaterialName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    PriceType = m.PriceType.ToString(),
                    Price = m.Price,
                    Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                    DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                    Discount = m.Discount,
                    ExtraDiscount = m.ExtraDiscount,
                    IsNew = m.IsNew,
                    IsFeatured = m.IsFeatured,
                    IsSpecial = m.IsSpecial,
                    Actived = m.Actived,
                    Views = m.Views,
                    CreateAt = m.CreateAt.GetValueOrDefault(),
                    PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
                }).ToList().OrderBy(p => p.ProductName).ToList();
            }
            return model;
        }

        public IEnumerable<ProductViewModel> GetProductViewModelsByCategory(string categoryName)
        {
            IEnumerable<ProductViewModel> model = null;
            try
            {
                model = this.All.Where(p => p.Category.CategoryName == categoryName).Select(m => new ProductViewModel
                {
                    Id = m.Id,
                    BasketCount = m.BasketCount,
                    BuyCount = m.BuyCount,
                    ProductCode = m.ProductCode,
                    ProductName = m.ProductName,
                    Slug = m.Slug,
                    Description = m.Description,
                    ProductTypeId = m.ProductTypeId,
                    ProductTypeName = m.ProductType.TypeName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.Material.MaterialName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    PriceType = m.PriceType.ToString(),
                    Price = m.Price,
                    Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                    DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                    Discount = m.Discount,
                    ExtraDiscount = m.ExtraDiscount,
                    IsNew = m.IsNew,
                    IsFeatured = m.IsFeatured,
                    IsSpecial = m.IsSpecial,
                    Actived = m.Actived,
                    Views = m.Views,
                    CreateAt = m.CreateAt.GetValueOrDefault(),
                    PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }

        public IEnumerable<ProductViewModel> GetProductViewModelsByOrder(int value)
        {
            IEnumerable<ProductViewModel> model = null;
            try
            {
                model =  this.All.Select(m => new ProductViewModel
                {
                    Id = m.Id,
                    BasketCount = m.BasketCount,
                    BuyCount = m.BuyCount,
                    ProductCode = m.ProductCode,
                    ProductName = m.ProductName,
                    Slug = m.Slug,
                    Description = m.Description,
                    ProductTypeId = m.ProductTypeId,
                    ProductTypeName = m.ProductType.TypeName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.Material.MaterialName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    PriceType = m.PriceType.ToString(),
                    Price = m.Price,
                    Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                    DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                    Discount = m.Discount,
                    ExtraDiscount = m.ExtraDiscount,
                    IsNew = m.IsNew,
                    IsFeatured = m.IsFeatured,
                    IsSpecial = m.IsSpecial,
                    Actived = m.Actived,
                    Views = m.Views,
                    CreateAt = m.CreateAt.GetValueOrDefault(),
                    PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
                }).ToList().GetRange(0, value).OrderBy(p => p.ProductName).ToList(); 
            }
            catch (Exception)
            {
                model = this.All.Select(m => new ProductViewModel
                {
                    Id = m.Id,
                    BasketCount = m.BasketCount,
                    BuyCount = m.BuyCount,
                    ProductCode = m.ProductCode,
                    ProductName = m.ProductName,
                    Slug = m.Slug,
                    Description = m.Description,
                    ProductTypeId = m.ProductTypeId,
                    ProductTypeName = m.ProductType.TypeName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.Material.MaterialName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    PriceType = m.PriceType.ToString(),
                    Price = m.Price,
                    Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                    DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                    Discount = m.Discount,
                    ExtraDiscount = m.ExtraDiscount,
                    IsNew = m.IsNew,
                    IsFeatured = m.IsFeatured,
                    IsSpecial = m.IsSpecial,
                    Actived = m.Actived,
                    Views = m.Views,
                    CreateAt = m.CreateAt.GetValueOrDefault(),
                    PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
                }).ToList().OrderBy(p => p.ProductName).ToList();
            }
            return model;

        }

        public IEnumerable<ProductViewModel> GetProductViewModelsByPrice(int min, int max)
        {
            IEnumerable<ProductViewModel> model = null;
            try
            {
                model = this.All.Where(p => p.Price >= min && p.Price <= max).Select(m => new ProductViewModel
                {
                    Id = m.Id,
                    BasketCount = m.BasketCount,
                    BuyCount = m.BuyCount,
                    ProductCode = m.ProductCode,
                    ProductName = m.ProductName,
                    Slug = m.Slug,
                    Description = m.Description,
                    ProductTypeId = m.ProductTypeId,
                    ProductTypeName = m.ProductType.TypeName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.Material.MaterialName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.CategoryName,
                    PriceType = m.PriceType.ToString(),
                    Price = m.Price,
                     Star = m.ProductReviews.Where(p => p.ProductId == m.Id).Average(p => p.Star),
                    DisplayImages = m.ProductImages.OrderBy(s => s.CreateAt).Select(s => s.Url).ToList(),
                    Discount = m.Discount,
                    ExtraDiscount = m.ExtraDiscount,
                    IsNew = m.IsNew,
                    IsFeatured = m.IsFeatured,
                    IsSpecial = m.IsSpecial,
                    Actived = m.Actived,
                    Views = m.Views,
                    CreateAt = m.CreateAt.GetValueOrDefault(),
                    PriceAfter = Math.Round((double)((double)m.Price.GetValueOrDefault() * (1 - ((m.Discount + m.ExtraDiscount) / 100))), 1, MidpointRounding.AwayFromZero)
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public IEnumerable<ProductViewModel> SortProductViewModels(string value)
        {
            IEnumerable<ProductViewModel> model = null;
            try
            {
                switch (value)
                {
                    case "Position Desc":
                        {
                            model = GetProductViewModels().OrderByDescending(p => p.ProductName);
                            break;
                        }
                    case "Name (Z - A)":
                        {
                            model = GetProductViewModels().OrderByDescending(p => p.ProductName);
                            break;
                        }
                    case "Price (Low > High)":
                        {
                            model = GetProductViewModels().OrderBy(p => p.Price.GetValueOrDefault());
                            break;
                        }
                    case "Price (High > Low)":
                        {
                            model = GetProductViewModels().OrderByDescending(p => p.Price.GetValueOrDefault());
                            break;
                        }
                    case "Rating (Highest)":
                        {
                            model = this.GetProductViewModels().OrderByDescending(p => p.Star);
                            break;
                        }
                    case "Rating (Lowest)":
                        {
                            model = GetProductViewModels().OrderBy(p => p.Star);
                            break;
                        }
                    default:
                        {
                            model = GetProductViewModels().OrderBy(p => p.ProductName);
                            break;
                        }
                       
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
