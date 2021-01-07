using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Component
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listcategory = await _categoryRepository.All.Where(x => x.CategoryId == null).ToListAsync();
            var listgroupby = listcategory.GroupBy(user => user.NoiThat);
            List<HienThiCategory> hienThiCategories = new List<HienThiCategory>();
            foreach (var group in listgroupby)
            {
                var category = new List<HienThiCategoryPhu>();
                foreach (var user in group)
                {
                    var listmenu = await _categoryRepository.All.Where(x => x.CategoryId == user.Id).ToListAsync();
                    var ls = new List<string>();
                    foreach (var variable in listmenu)
                    {
                        ls.Add(variable.CategoryName);
                    }
                    var menuphu = new HienThiCategoryPhu {Tencategory = user.CategoryName, Listcategoryphu = ls};
                    category.Add(menuphu);
                }

                var categoriiii = category.OrderByDescending(x => x.Listcategoryphu.Count);
                ViewBag.listcategory = await _categoryRepository.All.ToListAsync();
                var hienthi = new HienThiCategory {DeMuc = group.Key, Categoryy = categoriiii.ToList() };
                hienThiCategories.Add(hienthi);
            }

            ViewBag.HienthiCategories = hienThiCategories.OrderBy(x => x.Categoryy.Count).ToList();
            return View("Index");
        }
    }

    public class HienThiCategory
    {
        public string DeMuc;
        public List<HienThiCategoryPhu> Categoryy;
    }
    public class HienThiCategoryPhu
    {
        public string Tencategory;
        public List<string> Listcategoryphu;
    }
}
