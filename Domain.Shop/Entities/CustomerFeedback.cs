using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("CUSTOMER_FEEDBACK")]
    public class CustomerFeedback : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Column("INDEX")]
        public int Index { get; set; }

        [Column("CUSTOMER_ID")]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [Column("FEEDBACK")]
        public string Feedback { get; set; }
        public CustomerFeedbackImage CustomerFeedbackImage { get; set; }
    }
}
