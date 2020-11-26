using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities
{
    /// <summary>
    /// Hóa đơn nhập hàng.
    /// </summary>
    public class ImportBill
    {
        [NotMapped]
        private Supplier _supplier;

        /// <summary>
        /// Mã hóa đơn.
        /// </summary>
        [Key]
        public string Id { get; set; }

        //Unique check is done only in application layer (unique by each store).
        public string Code { get; set; }

        /// <summary>
        /// abc.
        /// </summary>
        [Display(Name = "Mã nhà cung cấp")]
        public int SupplierID { get; set; }
        [NotMapped]
        public Supplier Supplier
        {
            get => _supplier; set
            {
                _supplier = value;
                if (value != null) SupplierID = value.Id; //Để cho khi gán object thì gán luôn cả ID (khóa ngoại).
            }
        }

        public string StaffId { get; set; }

        /// <summary>
        /// Tổng giá trị hóa đơn bán.
        /// </summary>
        [Display(Name = "Tổng giá trị")]
        public int TotalValue { get; set; }

        /// <summary>
        /// Discount by entering coupon code.
        /// </summary>
        [Display(Name = "Lượng % giảm giá")]
        public int DiscountValue { get; set; }

        /// <summary>
        /// Số tiền cửa hàng của mình đã trả cho Supplier.
        /// </summary>
        public int Payment { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }


        [NotMapped]
        public virtual ICollection<ImportBillDetail> ImportBillDetails { get; set; }

        public void RefreshTotalValue()
        {
            TotalValue = GetTotalValue();
        }

        public int GetTotalValue()
        {
            int sum = 0;
            if (ImportBillDetails != null)
                foreach (var item in ImportBillDetails)
                {
                    if (item != null) sum += item.Amount * item.Price;
                    else ImportBillDetails.Remove(null);
                }
            return sum;
        }
    }
}