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
    }
}
