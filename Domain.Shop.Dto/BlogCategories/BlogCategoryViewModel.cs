using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace Domain.Shop.Dto.BlogCategories
{
    public class BlogCategoryViewModel
    {
        [MaxLength(50)]
        public string Id { get; set; }

        [MaxLength(255)]
        [Required]
        [DisplayName("Tên danh mục Blog")]
        public string BlogCategoryName { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Tên Slug")]
        public string Slug { get; set; }
        public string HierarchyCode { get; set; }

        [DisplayName("Danh mục Blog cha")]
        public string ParentHierarchyCode { get; set; }

        public IEnumerable<BlogCategoryViewModel> Childs { get; set; }
        public IEnumerable<BlogCategoryViewModel> Path { get; set; }

        public static IEnumerable<BlogCategoryViewModel> GetTreeBlogCategoryViewModels(IEnumerable<BlogCategoryViewModel> blogCategories, string HierarchyCode = null)
        {
            var result = new List<BlogCategoryViewModel>();
            if (string.IsNullOrEmpty(HierarchyCode))
            {
                result = blogCategories.Where(p => p.HierarchyCode.Length == Domain.Common.Consts.Infrastructure.HierarchyCodeLength)
                    .Select(p => new BlogCategoryViewModel
                    {
                        Id = p.Id,
                        BlogCategoryName = p.BlogCategoryName,
                        Slug = p.Slug,
                        HierarchyCode = p.HierarchyCode,
                        ParentHierarchyCode = HierarchyCode,
                        Childs = GetTreeBlogCategoryViewModels(blogCategories, p.HierarchyCode),
                        Path = blogCategories.Where(s => p.HierarchyCode.StartsWith(s.HierarchyCode)).OrderBy(s => s.HierarchyCode)
                    }).ToList();
            }
            else
            {
                result = blogCategories.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length - HierarchyCode.Length == Domain.Common.Consts.Infrastructure.HierarchyCodeLength)
                    .Select(p => new BlogCategoryViewModel
                    {
                        Id = p.Id,
                        BlogCategoryName = p.BlogCategoryName,
                        Slug = p.Slug,
                        HierarchyCode = p.HierarchyCode,
                        ParentHierarchyCode = HierarchyCode,
                        Childs = GetTreeBlogCategoryViewModels(blogCategories, p.HierarchyCode),
                        Path = blogCategories.Where(s => p.HierarchyCode.StartsWith(s.HierarchyCode)).OrderBy(s => s.HierarchyCode)
                    }).ToList();
            }
            return result;
        }
    }
}
