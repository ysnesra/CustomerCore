using CustomerCore.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CustomerCore.ViewModels.OrderModels.EditOrderOrderDetailsViewModel;

namespace CustomerCore.ViewModels
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        public StatusEnum Status { get; set; }
        public int AddressId { get; set; }
        public List<int> OrderDetailsId { get; set; } = new List<int>();

    }
 
}
