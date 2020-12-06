namespace Domain.Shop.Dto.ImportBill
{
    public class DetailImportModel
    {
        public int IdDetailImport { get; set; }

        public int? IdImport { get; set; }

        public string IdProduct { get; set; }

        public string NameProduct { get; set; }

        public decimal? Price { get; set; }

        public int? Amount { get; set; }

    }    
}