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
        public virtual ICollection<Product> Products { get; set; }
    }
}
