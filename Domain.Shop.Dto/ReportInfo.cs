using System;

namespace Domain.Shop.Dto
{
    public class ReportInfo
    {
        public int iGroup { get; set; }
        public String Group { get; set; }
        public int Count { get; set; }
        public long Sum { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
        public double Avg { get; set; }
    }
}
