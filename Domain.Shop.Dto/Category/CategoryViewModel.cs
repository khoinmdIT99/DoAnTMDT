using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain.Shop.Dto.Category
{
    public class CategoryViewModel
    {
        [MaxLength(50)]
        public string Id { get; set; }	
		[MaxLength(50)]
        [DisplayName("Đường dẫn")]
        public string Slug { get; set; }
        [MaxLength(255)]
        [DisplayName("Tên danh mục")]
		public string CategoryName { get; set; }
        public string HierarchyCode { get; set; }
        [DisplayName("Menu cha")]
        public string ParentHierarchyCode { get; set; }
        public IEnumerable<CategoryViewModel> Childs { get; set; }
		public static IEnumerable<CategoryViewModel> GetTreeMenuViewModels(IEnumerable<CategoryViewModel> categories, string HierarchyCode = null)
		{
			var result = new List<CategoryViewModel>();
			if (string.IsNullOrEmpty(HierarchyCode))
			{
				result = categories.Where(p => p.HierarchyCode.Length == Domain.Common.Consts.Infrastructure.HierarchyCodeLength)
					.Select(p => new CategoryViewModel
					{
						Id = p.Id,
						CategoryName = p.CategoryName,
						Slug= p.Slug,
						HierarchyCode = p.HierarchyCode,
						ParentHierarchyCode = HierarchyCode,
						Childs = GetTreeMenuViewModels(categories, p.HierarchyCode),
			        }).ToList();
            }
            else
            {

				result = categories.Where(p => p.HierarchyCode.StartsWith(HierarchyCode) && p.HierarchyCode.Length - HierarchyCode.Length == Domain.Common.Consts.Infrastructure.HierarchyCodeLength).Select(
				 c => new CategoryViewModel()
                 {
					 Id = c.Id,
					 CategoryName = c.CategoryName,
					 Slug = c.Slug,
					 HierarchyCode = c.HierarchyCode,
					 ParentHierarchyCode = HierarchyCode,
					 Childs = GetTreeMenuViewModels(categories, c.HierarchyCode),
				 }).ToList();
            }
			return result;
		}
	}
}
