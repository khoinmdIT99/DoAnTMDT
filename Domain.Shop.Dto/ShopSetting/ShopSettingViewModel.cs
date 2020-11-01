using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Dto.ShopSetting
{
    public class ShopSettingViewModel
    {
        public ShopSettingViewModel(string id, string pageName, string pagetitle, string pageDescription, string keyword)
        {
            Id = id;
            PageName = pageName;
            Pagetitle = pagetitle;
            PageDescription = pageDescription;
            Keyword = keyword;
        }

        public ShopSettingViewModel()
        {
        }

        public string Id { get; set; }

        [DisplayName("Tên Page")]
        [Required]
        public string PageName { get; set; }

        [DisplayName("Tiêu đề")]
        [Required]
        public string Pagetitle { get; set; }

        [DisplayName("Mô tả")]
        public string PageDescription { get; set; }

        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Mã số thuế")]
        public int? TaxPercent { get; set; }
    }
}
