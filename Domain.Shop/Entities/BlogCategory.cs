using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("BLOG_CATEGORIES")]
    public class BlogCategory : BaseEntity
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("SLUG")]
        public string Slug { get; set; }
        [MaxLength(255)]
        [Column("BLOG_CATEGORY_NAME")]
        public string BlogCategoryName { get; set; }
        [Column("HIERARCHY_CODE")]
        public string HierarchyCode { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
