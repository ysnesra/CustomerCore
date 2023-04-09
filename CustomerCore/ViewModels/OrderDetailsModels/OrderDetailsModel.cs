namespace CustomerCore.ViewModels
{
    public class OrderDetailsModel
    {  
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ItemId { get; set; }
        public int? Amount { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SalePrice { get; set; }

    }
}
