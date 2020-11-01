using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Dto.ProductTag
{
    public class ProductTagViewModel
    {
        public string Id { get; set; }

        [Required]
        public string TagId { get; set; }

        [Required]
        public string ProductId { get; set; }

        public string TagName { get; set; }
    }
}
