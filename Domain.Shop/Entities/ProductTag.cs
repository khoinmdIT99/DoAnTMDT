using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("PRODUCT_TAG")]
    public class ProductTag
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Required]
        [Column("TAG_ID")]
        public string TagId { get; set; }
        public Tag Tag { get; set; }
        [MaxLength(50)]
        [Required]
        [Column("PRODUCT_ID")]
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
