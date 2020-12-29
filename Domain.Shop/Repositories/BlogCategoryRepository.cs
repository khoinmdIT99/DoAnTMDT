using Domain.Shop.Dto.BlogCategories;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class BlogCategoryRepository : Repository<ShopDBContext, BlogCategory>, IBlogCategoryRepository
    {
        private readonly int _hierarchyCodeLength = Common.Consts.Infrastructure.HierarchyCodeLength;
        private readonly string _hierarchyCodeTemplate = Common.Consts.Infrastructure.HierarchyCodeTemplate;
        public BlogCategoryRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }
        public bool CanDeleteBlogCategory(string hierarchyCode)
        {
            return !this.All.Where(p => p.HierarchyCode.StartsWith(hierarchyCode) && p.HierarchyCode.Length > hierarchyCode.Length).Any();
        }

        public string GenerateHierarchyCode(string hierarchyCodeParent)
        {
            var query = this.All;
            if (string.IsNullOrEmpty(hierarchyCodeParent))
                query = query.Where(p => p.HierarchyCode.Length == _hierarchyCodeLength);
            else
                query = query.Where(p => p.HierarchyCode.StartsWith(hierarchyCodeParent) && p.HierarchyCode.Length == hierarchyCodeParent.Length + _hierarchyCodeLength);
            var maxHierarchyCode = query.OrderByDescending(p => p.HierarchyCode).Select(p => p.HierarchyCode).FirstOrDefault();
            long MaxHierarchyNumber = 0;
            if (!string.IsNullOrEmpty(maxHierarchyCode))
            {
                maxHierarchyCode = maxHierarchyCode.Substring(maxHierarchyCode.Length - _hierarchyCodeLength);
            }
            if (long.TryParse(maxHierarchyCode, out MaxHierarchyNumber))
            {
                MaxHierarchyNumber++;
            }
            return hierarchyCodeParent + string.Format(_hierarchyCodeTemplate, MaxHierarchyNumber);
        }

        public List<BlogCategory> GetChildBlogCategories(string hierarchyCode)
        {
            return this.All.Where(p => p.HierarchyCode.StartsWith(hierarchyCode) && p.HierarchyCode.Length > hierarchyCode.Length).ToList();
        }

        public BlogCategoryViewModel GetBlogCategoryViewModel(string id)
        {
            return this.All.Where(p => p.Id == id).Select(p => new BlogCategoryViewModel
            {
                Id = p.Id,
                BlogCategoryName = p.BlogCategoryName,
                Slug = p.Slug,
                HierarchyCode = p.HierarchyCode
            }).FirstOrDefault();
        }

        public IEnumerable<BlogCategoryViewModel> GetBlogCategoryViewModels()
        {
            var blogCategoryList = this.All.Select(p => new BlogCategoryViewModel
            {
                Id = p.Id,
                BlogCategoryName = p.BlogCategoryName,
                Slug = p.Slug,
                HierarchyCode = p.HierarchyCode
            }).OrderBy(p => p.HierarchyCode).ToList();
            return blogCategoryList;
        }

        public bool CheckNewSlug(string slug)
        {
            var query = this.All.Count(p => p.Slug == slug);
            if (query > 0) return true;
            else return false;
        }
    }
}
