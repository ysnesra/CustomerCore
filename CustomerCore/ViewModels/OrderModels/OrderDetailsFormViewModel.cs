using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{
    public class OrderDetailsFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sipariş Id'si")]
        public int OrderId { get; set; }

        [Display(Name = "Ürün Id'si")]
        public int ItemId { get; set; }

        [Display(Name = "Ürün Miktarı")]
        public int? Amount { get; set; }

        [Display(Name = "Birim Fiyatı")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Satış Fiyatı")]
        public decimal? SalePrice { get; set; }
        public List<OrderModel> OrderVM { get; set; }
        public List<ItemModel> ItemVM { get; set; }
    }
}
