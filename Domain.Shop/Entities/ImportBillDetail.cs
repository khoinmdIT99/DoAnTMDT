using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities
{
    /// <summary>
    /// Chi tiết hóa đơn nhập hàng
    /// </summary>
    public class ImportBillDetail
    {
        [Key]
        public int IdDetailImport { get; set; }

        public string IdProduct { get; set; }

        public decimal? Price { get; set; }

        public int? Amount { get; set; }
        [ForeignKey("ImportBill")]
        public int? IdImport { get; set; }
        public virtual ImportBill ImportBill { get; set; }
    }
}