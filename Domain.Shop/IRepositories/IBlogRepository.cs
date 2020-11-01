using Domain.Shop.Dto.Blogs;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IBlogRepository : IRepository<Blog>
    {
        IEnumerable<BlogViewModel> GetBlogsViewModel();
        BlogViewModel GetBlogViewModel(string id);
        BlogViewModel GetBlogViewModelBySlug(string slug);
    }
}
