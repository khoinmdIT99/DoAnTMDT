using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Domain.Shop.Entities
{
    /// <summary>
    /// Hóa đơn nhập hàng.
    /// </summary>
    [Table("ImportBill")]
    public class ImportBill
    {
        public ImportBill()
        {
            DetailImports = new HashSet<ImportBillDetail>();
        }

        [Key]
        public int IdImport { get; set; }
        public int? Amount { get; set; }
        public virtual ICollection<ImportBillDetail> DetailImports { get; set; }
        [ForeignKey("Supplier")]
        public int? IdSupplier { get; set; }
        public virtual Supplier Supplier { get; set; }
        public string StaffId { get; set; }

        /// <summary>
        /// Tổng giá trị hóa đơn bán.
        /// </summary>
        [Display(Name = "Tổng giá trị")]
        public decimal TotalValue { get; set; }
        /// <summary>
        /// Số tiền cửa hàng của mình đã trả cho Supplier.
        /// </summary>
        public decimal Payment { get; set; }
        /// <summary>
        /// Số tiền cửa hàng của mình nợ cho Supplier.
        /// </summary>
        public decimal TienNo { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Display(Name = "Ngày tạo")]
        public string DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public bool IsPaymentOk { get; set; }
        public void RefreshTotalValue()
        {
            TotalValue = GetTotalValue();
            Amount = GetAmountValue();
            TienNo = TotalValue;
            DateCreated = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            StartDate = DateTime.Now;
            Payment = 0;
        }

        public decimal GetTotalValue()
        {
            decimal sum = 0;
            if (DetailImports != null)
                foreach (var item in DetailImports)
                {
                    if (item != null) sum += item.Amount.GetValueOrDefault() * item.Price.GetValueOrDefault();
                    else DetailImports.Remove(null);
                }
            return sum;
        }
        public int GetAmountValue()
        {
            int sum = 0;
            if (DetailImports != null)
                foreach (var item in DetailImports)
                {
                    if (item != null) sum += item.Amount.GetValueOrDefault();
                    else DetailImports.Remove(null);
                }
            return sum;
        }
    }
}