using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("PRODUCT_TYPES")]
    public class ProductType
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("TYPE_NAME")]
        [Required]
        public string TypeName { get; set; }
        [StringLength(2000, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [StringLength(255)]
        public string SeoAlias { set; get; }

        [StringLength(255)]
        public string SeoKeywords { set; get; }

        [StringLength(255)]
        public string SeoDescription { set; get; }
        public virtual  ICollection<Product> Products { get; set; }
    }
}
