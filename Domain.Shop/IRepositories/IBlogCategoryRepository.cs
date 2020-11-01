using Domain.Shop.Dto.BlogCategories;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IBlogCategoryRepository : IRepository<BlogCategory>
    {
        IEnumerable<BlogCategoryViewModel> GetBlogCategoryViewModels();
        BlogCategoryViewModel GetBlogCategoryViewModel(string Id);
        string GenerateHierarchyCode(string HierarchyCodeParent);

        bool CanDeleteBlogCategory(string HierarchyCode);
        public List<BlogCategory> GetChildBlogCategories(string HierarchyCode);

        bool CheckNewSlug(string Slug);
    }
}
