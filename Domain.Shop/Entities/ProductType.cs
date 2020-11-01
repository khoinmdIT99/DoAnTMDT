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
        public virtual  ICollection<Product> Products { get; set; }
    }
}
