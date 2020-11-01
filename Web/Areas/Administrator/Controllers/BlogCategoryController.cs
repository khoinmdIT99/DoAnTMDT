using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Shop.Dto.BlogCategories;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class BlogCategoryController : BaseController
    {
        private IBlogCategoryRepository blogCategoryRepository;
        public BlogCategoryController(IBlogCategoryRepository blogCategoryRepository)
        {
            this.blogCategoryRepository = blogCategoryRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<BlogCategoryViewModel> blogCategories = blogCategoryRepository.GetBlogCategoryViewModels();
            blogCategories = BlogCategoryViewModel.GetTreeBlogCategoryViewModels(blogCategories);
            return View(blogCategories);
        }
        private void SetComboData(string HierarchyCode = null)
        {
            var blogCategoryQuery = blogCategoryRepository.All;
            if (!string.IsNullOrEmpty(HierarchyCode))
            {
                blogCategoryQuery = blogCategoryQuery.Where(p => !p.HierarchyCode.StartsWith(HierarchyCode));
            }
            ViewBag.blogCategories = blogCategoryQuery.OrderBy(p => p.HierarchyCode).Select(p => new SelectListItem
            {
                Text = p.BlogCategoryName,
                Value = p.HierarchyCode
            }).ToList();
        }
        public ActionResult Create()
        {
            SetComboData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!blogCategoryRepository.CheckNewSlug(model.Slug))
                    {
                        model.Id = Guid.NewGuid().ToString();
                        BlogCategory blogCategory = new BlogCategory();
                        PropertyCopy.Copy(model, blogCategory);
                        blogCategory.HierarchyCode = blogCategoryRepository.GenerateHierarchyCode(model.ParentHierarchyCode);
                        blogCategoryRepository.Add(blogCategory);
                        blogCategoryRepository.Save(RequestContext);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetComboData();
                        return View();
                    }
                }
                catch (Exception)
                {
                    SetComboData();
                    return View();
                }
            }
            SetComboData();
            return View();
        }
        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                BlogCategory blogCategory = blogCategoryRepository.Single(p => p.Id == id, null);
                if (blogCategoryRepository.CanDeleteBlogCategory(blogCategory.HierarchyCode))
                {
                    blogCategoryRepository.Delete(blogCategory);
                    blogCategoryRepository.Save(RequestContext);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception )
            {
                return false;
            }

        }
        public ActionResult Update(string id)
        {
            var model = blogCategoryRepository.GetBlogCategoryViewModel(id);
            SetComboData(model.HierarchyCode);
            model.ParentHierarchyCode = model.HierarchyCode.Substring(0, model.HierarchyCode.Length - Domain.Common.Consts.Infrastructure.HierarchyCodeLength);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(BlogCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!blogCategoryRepository.CheckNewSlug(model.Slug))
                    {
                        BlogCategory blogCategory = blogCategoryRepository.Single(p => p.Id == model.Id, null);
                        List<string> excludeProperty = new List<string>() { "Id", "HierarchyCode" };
                        PropertyCopy.Copy(model, blogCategory, excludeProperty);
                        string currentHierarchyParent = blogCategory.HierarchyCode.Substring(0, blogCategory.HierarchyCode.Length - Domain.Common.Consts.Infrastructure.HierarchyCodeLength);
                        if (string.IsNullOrEmpty(model.ParentHierarchyCode) ? !string.IsNullOrEmpty(currentHierarchyParent) : model.ParentHierarchyCode != currentHierarchyParent)
                        {
                            var hierarchyCode = blogCategoryRepository.GenerateHierarchyCode(model.ParentHierarchyCode);
                            var childBlogCategories = blogCategoryRepository.GetChildBlogCategories(blogCategory.HierarchyCode);
                            foreach (BlogCategory item in childBlogCategories)
                            {
                                item.HierarchyCode = model.ParentHierarchyCode + item.HierarchyCode.Substring(hierarchyCode.Length);
                            }
                            blogCategory.HierarchyCode = hierarchyCode;
                        }
                        blogCategoryRepository.Update(blogCategory);
                        blogCategoryRepository.Save(RequestContext);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetComboData();
                        return View();
                    }
                }
                catch (Exception)
                {
                    SetComboData(model.HierarchyCode);
                    return View();
                }
            }
            SetComboData(model.HierarchyCode);
            return View();
        }
    }
}
