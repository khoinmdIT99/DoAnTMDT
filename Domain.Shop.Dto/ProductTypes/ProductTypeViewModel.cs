using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Shop.Dto.ProductTypes
{
    public class ProductTypeViewModel
    {
        public string Id { get; set; }

        [DisplayName("Tên loại sản phẩm")]
        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; }
        [StringLength(2000, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
