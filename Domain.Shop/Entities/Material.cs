using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{

    [Table("MATERIALS")]
    public class Material
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("MATERIAL_NAME")]
        public string MaterialName { get; set; }
        [MaxLength(255)]
        [Column("NOTE")]
        public string Note { get; set; }
        [StringLength(255)]
        public string SeoAlias { set; get; }

        [StringLength(255)]
        public string SeoKeywords { set; get; }

        [StringLength(255)]
        public string SeoDescription { set; get; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
