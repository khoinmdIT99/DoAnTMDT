using Domain.Shop.Dto.Customer;
using Domain.Shop.Dto.Products;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.Dto.ProductReview
{
    public class ProductReviewViewModel : BaseEntity
    {
      
            public string Id { get; set; }
            
            public string Name { get; set; }
         
            public string Title { get; set; }
          
            public string Review { get; set; }
            public int Star { get; set; }
            public bool Approved { get; set; }
            public string ProductId { get; set; }
            public virtual ProductViewModel Product { get; set; }
            public string CustomerId { get; set; }
            public virtual CustomerViewModel Customer { get; set; }
        }
    
}
