using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.BlogCategories;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Component
{
    public class BlogViewComponent:ViewComponent
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;

        public BlogViewComponent(IBlogCategoryRepository blogCategoryRepository)
        {
            _blogCategoryRepository = blogCategoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<BlogCategoryViewModel> blogCategories = _blogCategoryRepository.GetBlogCategoryViewModels();
            blogCategories = BlogCategoryViewModel.GetTreeBlogCategoryViewModels(blogCategories);
            var listcategory = blogCategories.ToList();
            var listtotal = new ListTotal();
            listtotal.list1 = listcategory.First().Childs.ToList();
            listtotal.list2 = listcategory[1].Childs.ToList();
            return View("Index",listtotal);
        }
    }
    public class ListTotal
    {
        public List<BlogCategoryViewModel> list1;
        public List<BlogCategoryViewModel> list2;
    }
}
