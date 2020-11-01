using Domain.Shop.Dto.Blogs;
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
    public class BlogRepository : Repository<ShopDBContext, Blog>, IBlogRepository
    {
        public BlogRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<BlogViewModel> GetBlogsViewModel()
        {
            return this.All.Select(b => new BlogViewModel
            {
                Id = b.Id,
                Slug = b.Slug,
                Title = b.Title,
                Content = b.Content
            }).ToList();
        }
        public BlogViewModel GetBlogViewModel(string id)
        {
            return this.All.Where(b => b.Id == id).Select(b => new BlogViewModel
            {
                Id = b.Id,
                Slug = b.Slug,
                Title = b.Title,
                Content = b.Content
            }).FirstOrDefault();
        }

        public BlogViewModel GetBlogViewModelBySlug(string slug)
        {
            return this.All.Where(b => b.Slug == slug).Select(b => new BlogViewModel
            {
                Id = b.Id,
                Slug = b.Slug,
                Title = b.Title,
                Content = b.Content
            }).FirstOrDefault();
        }
    }
}
