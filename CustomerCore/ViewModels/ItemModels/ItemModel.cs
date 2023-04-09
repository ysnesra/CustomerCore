using CustomerCore.Context;
using CustomerCore.ViewModels.PhotoModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{
    public class ItemModel
    {
        public int Id { get; set; }

        public string ItemCode { get; set; }
      
        public string Item { get; set; }

        public decimal UnitPrice { get; set; }
    
        public int CategoryId { get; set; }

        public int VatRateId { get; set; }
       
        public string Brand { get; set; }
       
        public int Amount { get; set; }


        public List<string> PhotoFiles { get; set; } = new List<string>(); 
    }
}
