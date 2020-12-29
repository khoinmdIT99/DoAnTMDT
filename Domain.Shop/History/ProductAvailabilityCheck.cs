using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shop.History
{
    public class ProductAvailabilityCheck
    {
        [Key, Column(Order = 0)]
        public string ProductID { get; set; }

        [Key, Column(Order = 1)]
        public DateTime Date { get; set; }

        public int AmountExpected { get; set; }

        public int AmountChecked { get; set; }

        public bool Checked { get; set; }        

        public int NumberOfPoorQuality { get; set; }

        public int NumberOfLost { get; set; }

        public int NumberOfExcess { get; set; }

        //At application layer should check : AmountChecked == AmountExpected - NumberOfPoorQuality - NumberOfLost + NumberOfExcess.
    }
}