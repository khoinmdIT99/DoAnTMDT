using Domain.Shop.Dto.BlogCategories;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Component
{
    public class BlogCategoryViewComponent : ViewComponent
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;

        public BlogCategoryViewComponent(IBlogCategoryRepository blogCategoryRepository)
        {
            this._blogCategoryRepository = blogCategoryRepository;
        }
        public IViewComponentResult Invoke()
        {
            IEnumerable<BlogCategoryViewModel> blogCategories = _blogCategoryRepository.GetBlogCategoryViewModels();
            blogCategories = BlogCategoryViewModel.GetTreeBlogCategoryViewModels(blogCategories);
            return View(blogCategories);
        }
    }
}
