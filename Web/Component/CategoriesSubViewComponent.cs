using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Component
{
    public class CategoriesSubViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesSubViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listcategory = await _categoryRepository.All.Where(x => x.CategoryId != null).ToListAsync();
            var listgroupby = listcategory.GroupBy(user => user.NoiThat);
            var result = listcategory.GroupBy(x => x.NoiThat)
                .Select(g => g.Count())
                .Max();
            var rangeTotals = listcategory.GroupBy(x => new { x.CategoryId })
                .Select(g => new CategoryMax()
                {
                    id = g.Key.CategoryId,
                    ten = _categoryRepository.All.FirstOrDefault(x => x.Id == g.Key.CategoryId)?.CategoryName,
                    ToTal = g.Count()
                }).OrderByDescending(x => x.ToTal).ToList();
            ViewBag.HotCategoryName = rangeTotals.First().ten;
            ViewBag.HotCategoryList = await _categoryRepository.All.Where(x => x.CategoryId == rangeTotals.First().id).ToListAsync();
            var sub1 = rangeTotals[new Random().Next(1, rangeTotals.Count)];
            var sub2 = rangeTotals[new Random().Next(1, rangeTotals.Count)];
            var sub3 = rangeTotals[new Random().Next(1, rangeTotals.Count)];
            var sub4 = rangeTotals[new Random().Next(1, rangeTotals.Count)];
            ViewBag.sub1Name = sub1.ten;
            ViewBag.sub2Name = sub2.ten;
            ViewBag.sub3Name = sub3.ten;
            ViewBag.sub4Name = sub4.ten;
            ViewBag.sub1List = await _categoryRepository.All.Where(x => x.CategoryId == sub1.id).ToListAsync();
            ViewBag.sub2List = await _categoryRepository.All.Where(x => x.CategoryId == sub2.id).ToListAsync();
            ViewBag.sub3List = await _categoryRepository.All.Where(x => x.CategoryId == sub3.id).ToListAsync();
            ViewBag.sub4List = await _categoryRepository.All.Where(x => x.CategoryId == sub4.id).ToListAsync();
            return View("Index");
        }
    }

    public class CategoryMax
    {
        public string id;
        public string ten;
        public int ToTal;
    }
}
