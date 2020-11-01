using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.Dto.ProductImage
{
    public class ProductImageViewModel
    {
        public string Id { get; set; } 
        public string ProductId { get; set; }
        public IFormFile Url { get; set; }
    }
}
