using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("CATEGORIES")]
    public class Category : BaseEntity
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("SLUG")]
        public string Slug { get; set; }
        [MaxLength(255)]
        [Column("CATEGORY_NAME")]
        public string CategoryName { get; set; }
        [Column("HIERARCHY_CODE")]
        public string HierarchyCode { get; set; }
        [MinLength(2)]
        [MaxLength(1000)]
        public string Description { get; set; }
        public string NoiThat { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Sub Category")]
        [MaxLength(50)]
        public string CategoryId { get; set; }
        [StringLength(255)]
        public string SeoAlias { set; get; }

        [StringLength(255)]
        public string SeoKeywords { set; get; }

        [StringLength(255)]
        public string SeoDescription { set; get; }
        public ICollection<Product> Products { get; set; }
    }
}
