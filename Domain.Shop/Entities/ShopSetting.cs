using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("SHOP_SETTING")]
    public class ShopSetting
    {
        [Key]
        [Column("ID")]
        [Required]
        public string Id { get; set; }
        [Column("PAGE_NAME")]
        [Required]
        public string PageName { get; set; }
        [Column("PAGE_TITLE")]
        [Required]
        public string Pagetitle { get; set; }
        [Column("PAGE_DESCRIPTION")]
        public string PageDescription { get; set; }
        [Column("KEYWORD")]
        public string Keyword { get; set; }
        public List<ShopAddress> ShopAddresses { get; set; }
        [Column("TAX_PERCENT")]
        public int? TaxPercent { get; set; }
    }
}
