using Domain.Shop.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Component
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductCategoryViewComponent(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        public IViewComponentResult Invoke()
        {
            return View(_categoryRepository.GetCategoryViewModels());
        }

    }
}
