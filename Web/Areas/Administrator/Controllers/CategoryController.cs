﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.Category;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;

        }

        public ViewResult Index()
        {
            IEnumerable<CategoryViewModel> categories = _categoryRepository.GetCategoryViewModels();
            categories = CategoryViewModel.GetTreeMenuViewModels(categories);
            return View(categories);
        }

        private void SetComboData(string HierarchyCode = null)
        {
            var categoryQuery = _categoryRepository.All;
            if (!string.IsNullOrEmpty(HierarchyCode))
            {
                categoryQuery = categoryQuery.Where(p => !p.HierarchyCode.StartsWith(HierarchyCode));
            }
            ViewBag.categories = categoryQuery.OrderBy(p => p.HierarchyCode).Select(p => new SelectListItem
            {
                Text = p.CategoryName,
                Value = p.HierarchyCode
            }).ToList();
        }

        public async Task<string> FindId(string name)
        {
            var categoryQuery = await _categoryRepository.All.ToListAsync();
            return categoryQuery.FirstOrDefault(x => x.CategoryName == name)?.Id;
        }

        public ViewResult Create()
        {
            SetComboData();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (!_categoryRepository.CheckSlugExist(model.Slug)) {
                        model.Id = Guid.NewGuid().ToString();
                        Category category = new Category();
                        PropertyCopy.Copy(model, category);
                        category.Id = Guid.NewGuid().ToString();
                        category.HierarchyCode = _categoryRepository.GenerateHierarchyCode(model.ParentHierarchyCode);
                        category.SeoAlias = TextHelper.ToUnsignString(model.CategoryName);
                        var listcategory = await _categoryRepository.All.OrderBy(p => p.HierarchyCode).Select(p => new SelectListItem
                        {
                            Text = p.CategoryName,
                            Value = p.HierarchyCode
                        }).ToListAsync();
                        category.CategoryId = await FindId(listcategory.FirstOrDefault(x => x.Value == model.ParentHierarchyCode)?.Text);
                        await _categoryRepository.AddAsync(category);
                        await _categoryRepository.SaveAsync(RequestContext);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetComboData();
                        return View();
                    }
                }
                catch (Exception )
                { 
                    SetComboData();
                    return View();
                }
            }
            else
            {
                SetComboData();
                return View();
            }
        }
        public ActionResult Update(string id)
        {
            var model = _categoryRepository.GetMenuViewModel(id);
            SetComboData(model.HierarchyCode);
            model.ParentHierarchyCode = model.HierarchyCode.Substring(0, model.HierarchyCode.Length - Domain.Common.Consts.Infrastructure.HierarchyCodeLength);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_categoryRepository.CheckSlugExist(model.Slug))
                    {
                        Category category = _categoryRepository.Single(p => p.Id == model.Id);
                        model.SeoAlias = TextHelper.ToUnsignString(category.CategoryName);
                        PropertyCopy.Copy(model, category);
                        string currentHierarchyParent = category.HierarchyCode.Substring(0, category.HierarchyCode.Length - Domain.Common.Consts.Infrastructure.HierarchyCodeLength);
                        if (string.IsNullOrEmpty(model.ParentHierarchyCode) ? !string.IsNullOrEmpty(currentHierarchyParent) : model.ParentHierarchyCode != currentHierarchyParent)
                        {
                            var hierarchyCode = _categoryRepository.GenerateHierarchyCode(model.ParentHierarchyCode);
                            var childMenus = _categoryRepository.GetChildMenus(category.HierarchyCode);
                            foreach (Category item in childMenus)
                            {
                                item.HierarchyCode = model.ParentHierarchyCode + item.HierarchyCode.Substring(hierarchyCode.Length);
                            }
                            category.HierarchyCode = hierarchyCode;
                        }
                        _categoryRepository.Update(category);
                        _categoryRepository.Save(RequestContext);
                    }
                    else
                    {
                        Category category = _categoryRepository.Single(p => p.Id == model.Id);
                        model.SeoAlias = TextHelper.ToUnsignString(category.CategoryName);
                        PropertyCopy.Copy(model, category);
                        _categoryRepository.Update(category);
                        _categoryRepository.Save(RequestContext);
                    }
                }
                catch (Exception )
                {
                    SetComboData(model.HierarchyCode);
                    return View();
                }
                return RedirectToAction("Index");
            }
            SetComboData(model.HierarchyCode);
            return View();
        }
        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                Category menu = _categoryRepository.Single(p => p.Id == id);
                if (_categoryRepository.CanDeleteMenu(menu.HierarchyCode))
                {
                    _categoryRepository.Delete(menu);
                    _categoryRepository.Save(RequestContext);  
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
    }
   
    
}
