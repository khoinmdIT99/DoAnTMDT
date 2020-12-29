using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Blogs;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            this._blogRepository = blogRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("tintuc.html/{slug?}", Name = "Blog-Detail")]
        public ViewResult GetBlog(string slug)
        {
            BlogViewModel model = _blogRepository.GetBlogViewModelBySlug(slug);
            if(model != null)
            {
                ViewBag.BlogExists = true;
                return View(model);
            }
            ViewBag.BlogExists = false;

            return View();
        }
    }
}
