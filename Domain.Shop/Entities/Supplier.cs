using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }


        [Column(TypeName = "varchar(10)")]
        [DisplayName("ICN")]
        public string Icn { get; set; }


        [Column(TypeName = "varchar(50)")]
        [DisplayName("Phone")]
        public string Phone { get; set; }


        [EmailAddress]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Email")]
        public string Email { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// Nếu &lt; 0 thì cửa hàng của ta đang nợ nhà cung cấp này. Nhớ là supplier.Money == importBill.Payment - importBill.TotalValue
        /// </summary>
        public int Money { get; set; }
    }
}