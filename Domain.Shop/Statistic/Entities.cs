using System;

namespace Domain.Shop.Statistic
{
    public class Date_Amount_MoneyInt
    {
        public string Date { get; set; }
        public int Amount { get; set; }
        public decimal Money { get; set; }
    } 

    public class Date_Amount_MoneyDouble
    {
        public string Date { get; set; }
        public double Money { get; set; }
    }

    public class ProductAvailabilityHistory
    {
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }

    public class SaleHistory
    {
        public string ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public long Price { get; set; }
    }

    public class Product_Amount
    {
        public string ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int SumAmount { get; set; }
    }

    public class MonthYear_Amount
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public long Amount { get; set; }
    }

    public class MonthYear_SumSaleImportRevenue
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalSale { get; set; }
        public int TotalImport { get; set; }
        public int TotalRevenue { get; set; }
    }
}