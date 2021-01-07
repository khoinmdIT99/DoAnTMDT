using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Blogs;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class BlogController : BaseController
    {
        private readonly IMemoryCache _cache;
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogCategoryRepository _blogCategoryRepository;

        public BlogController(IBlogRepository blogRepository, IBlogCategoryRepository blogCategoryRepository, IMemoryCache cache)
        {
            this._blogRepository = blogRepository;
            this._blogCategoryRepository = blogCategoryRepository;
            _cache = cache;
        }
        public ViewResult Index()
        {
            if (CheckBlogCategory())
            {
                ViewBag.CheckBlogCategory = true;
                if (_cache.TryGetValue("blogListAll", out List<BlogViewModel> bl))
                {
                    return View(bl);
                }

                var blogList = _blogRepository.GetBlogsViewModel();
                
                return View(blogList);
            }
            return View();
        }
        private bool CheckBlogCategory()
        {
            if (_blogCategoryRepository.GetBlogCategoryViewModels().Any())
            {
                return true;
            }
            return false;
        }
        public ViewResult Create()
        {
            ViewBag.slugs = _blogCategoryRepository.All.Select(b => new SelectListItem
            {
                Text = b.Slug,
                Value = b.Slug
            }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(BlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = Guid.NewGuid().ToString();
                    Blog blog = new Blog();
                    PropertyCopy.Copy(model, blog);
                    _blogRepository.Add(blog);
                    _blogRepository.Save(RequestContext);
                    _cache.Remove("blogListAll");
                    return RedirectToAction("Index");
                }
                catch (Exception )
                {
                    return View();
                }
            }
            return View();
        }
        public ViewResult Update(string id)
        {
            ViewBag.slugs = _blogCategoryRepository.All.Select(b => new SelectListItem
            {
                Text = b.Slug,
                Value = b.Slug
            }).ToList();
            return View(_blogRepository.GetBlogViewModel(id));
        }
        [HttpPost]
        public IActionResult Update(BlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Blog blog = new Blog();
                    PropertyCopy.Copy(model, blog);
                    _blogRepository.Update(blog);
                    _blogRepository.Save(RequestContext);
                    _cache.Remove("blogListAll");
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.slugs = _blogCategoryRepository.All.Select(b => new SelectListItem
                    {
                        Text = b.Slug,
                        Value = b.Slug
                    }).ToList();
                    return View();
                }
            }
            ViewBag.slugs = _blogCategoryRepository.All.Select(b => new SelectListItem
            {
                Text = b.Slug,
                Value = b.Slug
            }).ToList();
            return View();
        }
        public bool Delete(string id)
        {
            try
            {
                Blog blog = _blogRepository.All.FirstOrDefault(b => b.Id == id);
                _blogRepository.Delete(blog);
                _blogRepository.Save(RequestContext);
                _cache.Remove("blogListAll");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
