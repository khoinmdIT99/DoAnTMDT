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
        private readonly int HierarchyCodeLength = Domain.Common.Consts.Infrastructure.HierarchyCodeLength;
        private readonly string HierarchyCodeTemplate = Domain.Common.Consts.Infrastructure.HierarchyCodeTemplate;
        public BlogCategoryRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }
        public bool CanDeleteBlogCategory(string HierarchyCode)
        {
            return !this.All.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length > HierarchyCode.Length).Any();
        }

        public string GenerateHierarchyCode(string HierarchyCodeParent)
        {
            var query = this.All;
            if (string.IsNullOrEmpty(HierarchyCodeParent))
                query = query.Where(p => p.HierarchyCode.Length == HierarchyCodeLength);
            else
                query = query.Where(p => p.HierarchyCode.StartsWith(HierarchyCodeParent) && p.HierarchyCode.Length == HierarchyCodeParent.Length + HierarchyCodeLength);
            var MaxHierarchyCode = query.OrderByDescending(p => p.HierarchyCode).Select(p => p.HierarchyCode).FirstOrDefault();
            long MaxHierarchyNumber = 0;
            if (!string.IsNullOrEmpty(MaxHierarchyCode))
            {
                MaxHierarchyCode = MaxHierarchyCode.Substring(MaxHierarchyCode.Length - HierarchyCodeLength);
            }
            if (long.TryParse(MaxHierarchyCode, out MaxHierarchyNumber))
            {
                MaxHierarchyNumber++;
            }
            return HierarchyCodeParent + string.Format(HierarchyCodeTemplate, MaxHierarchyNumber);
        }

        public List<BlogCategory> GetChildBlogCategories(string HierarchyCode)
        {
            return this.All.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length > HierarchyCode.Length).ToList();
        }

        public BlogCategoryViewModel GetBlogCategoryViewModel(string Id)
        {
            return this.All.Where(p => p.Id == Id).Select(p => new BlogCategoryViewModel
            {
                Id = p.Id,
                BlogCategoryName = p.BlogCategoryName,
                Slug = p.Slug,
                HierarchyCode = p.HierarchyCode
            }).FirstOrDefault();
        }

        public IEnumerable<BlogCategoryViewModel> GetBlogCategoryViewModels()
        {
            var BlogCategoryList = this.All.Select(p => new BlogCategoryViewModel
            {
                Id = p.Id,
                BlogCategoryName = p.BlogCategoryName,
                Slug = p.Slug,
                HierarchyCode = p.HierarchyCode
            }).OrderBy(p => p.HierarchyCode).ToList();
            return BlogCategoryList;
        }

        public bool CheckNewSlug(string Slug)
        {
            var query = this.All.Where(p => p.Slug == Slug).Count();
            if (query > 0) return true;
            else return false;
        }
    }
}
