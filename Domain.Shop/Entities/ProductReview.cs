using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("PRODUCT_REVIEW")]
    public class ProductReview : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("TITLE")]
        public string Title { get; set; }
        [Column("REVIEW")]
        public string Review { get; set; }
        [Column("STAR")]
        public int Star { get; set; }
        [Column("APPROVED")]
        public bool Approved { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
