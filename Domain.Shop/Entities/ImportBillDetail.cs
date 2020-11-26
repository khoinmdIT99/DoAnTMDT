using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.Entities
{
    /// <summary>
    /// Chi tiết hóa đơn nhập hàng
    /// </summary>
    public class ImportBillDetail
    {
        /// <summary>
        /// Mã hóa đơn mà Chi tiết hóa đơn này kết nối tới. Đây là 1 phần của khóa chính.
        /// </summary>
        [Display(Name = "Mã hóa đơn nhập")]
        public string ImportBillId { get; set; }
        private ImportBill _importBillId;
        [NotMapped]
        public ImportBill ImportBill
        {
            get => _importBillId; set
            {
                _importBillId = value;
                if (value != null) ImportBillId = value.Id;
            }
        }

        /// <summary>
        /// Mã sản phẩm. Đây là 1 phần của khóa chính.
        /// </summary>
        [Display(Name = "Mã sản phẩm")]
        public string ProductId { get; set; }
        [NotMapped]
        private Product _product;
        [NotMapped]
        public Product Product
        {
            get => _product; set
            {
                _product = value;
                if (value != null) ProductId = value.Id;
            }
        }

        /// <summary>
        /// Giá nhập tại thời điểm lập hóa đơn.
        /// </summary>
        [Display(Name = "Đơn giá")]
        public int Price { get; set; }

        /// <summary>
        /// Số lượng.
        /// </summary>
        [Display(Name = "Số lượng")]
        public int Amount { get; set; }

        /// <summary>
        /// Mã nhà cung cấp.
        /// </summary>
        [Display(Name = "Mã nhà cung cấp")]
        public int SupplierId { get; set; }
        [NotMapped]
        private Supplier _supplier;
        [NotMapped]
        public Supplier Supplier
        {
            get => _supplier; set
            {
                _supplier = value;
                if (value != null) SupplierId = value.Id;
            }
        }
    }
}