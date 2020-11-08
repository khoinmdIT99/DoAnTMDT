using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    public class Slider
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }

        [MinLength(2)]
        [Display(Name = "Photo")]
        [StringLength(int.MaxValue)]
        public string PhotoName { get; set; }
        public bool Status { get; set; }
    }
}
