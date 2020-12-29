using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("TAGS")]
    public class Tag
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Required]
        [Column("NAME")]
        public string Name { get; set; }
        [StringLength(255)]
        public string SeoAlias { set; get; }

        [StringLength(255)]
        public string SeoKeywords { set; get; }

        [StringLength(255)]
        public string SeoDescription { set; get; }
    }
}
