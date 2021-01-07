using Domain.Shop.Dto.Blogs;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.Shop.Repositories
{
    public class BlogRepository : Repository<ShopDBContext, Blog>, IBlogRepository
    {
        private readonly IMemoryCache _cache;
        public BlogRepository(IUnitOfWork<ShopDBContext> unitOfWork, IMemoryCache cache) : base(unitOfWork)
        {
            _cache = cache;
        }

        public IEnumerable<BlogViewModel> GetBlogsViewModel()
        {
            if (_cache.TryGetValue("blogListAll", out List<BlogViewModel> bl))
            {
                return bl;
            }
            var listblog = All.Select(b => new BlogViewModel
            {
                Id = b.Id,
                Slug = b.Slug,
                Title = b.Title,
                Content = b.Content
            }).ToList();
            _cache.Set("blogListAll", listblog, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1),
                Priority = CacheItemPriority.Normal
            });
            return listblog;
        }
        public BlogViewModel GetBlogViewModel(string id)
        {
            if (_cache.TryGetValue("blogListAll", out List<BlogViewModel> bl))
            {
                return bl.Where(b => b.Id == id).Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Slug = b.Slug,
                    Title = b.Title,
                    Content = b.Content
                }).FirstOrDefault();
            }

            var listblogall = GetBlogsViewModel();
            return listblogall.Where(b => b.Id == id).Select(b => new BlogViewModel
            {
                Id = b.Id,
                Slug = b.Slug,
                Title = b.Title,
                Content = b.Content
            }).FirstOrDefault();
        }

        public BlogViewModel GetBlogViewModelBySlug(string slug)
        {
            if (_cache.TryGetValue("blogListAll", out List<BlogViewModel> bl))
            {
                return bl.Where(b => b.Slug == slug).Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Slug = b.Slug,
                    Title = b.Title,
                    Content = b.Content
                }).FirstOrDefault();
            }
            var listblogall = GetBlogsViewModel();
            return listblogall.Where(b => b.Slug == slug).Select(b => new BlogViewModel
            {
                Id = b.Id,
                Slug = b.Slug,
                Title = b.Title,
                Content = b.Content
            }).FirstOrDefault();
        }
    }
}
