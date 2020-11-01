using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.Tags
{
    public class TagViewModel
    {
        public string Id { get; set; }

        [DisplayName("Tên tag")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
