using CustomerCore.ViewModels.AddressModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CustomerCore.ViewModels.OrderModels.EditOrderOrderDetailsViewModel;

namespace CustomerCore.ViewModels.OrderModels
{
    public class OrderFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Müşteri Id'si")]
        public int CustomerId { get; set; }

        [Display(Name = "Sipariş Tarihi")]

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(Name = "Sipariş Durumu")]
        public StatusEnum Status { get; set; }

        [Display(Name = "Adres Id'si")]
        public int AddressId { get; set; }
        public List<CustomerModel> CustomerVM { get; set; }
        public List<AddressModel> AddressVM { get; set; }

        public List<OrderDetailsFormViewModel> OrderDetailsMModels { get; set; }   
        public OrderFormViewModel()              
        {                                                    
            OrderDetailsMModels = new List<OrderDetailsFormViewModel>();                        
        }
    }
}
