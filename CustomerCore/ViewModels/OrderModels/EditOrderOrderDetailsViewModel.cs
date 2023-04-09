
using CustomerCore.ViewModels.AddressModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels.OrderModels
{
    public class EditOrderOrderDetailsViewModel
    {
        public int OrderMId { get; set; }

        [Display(Name = "Müşteri Idsi")]
        [Required(ErrorMessage = "Departman İsmi zorunludur.")]
        public int OrderMCustomerId { get; set; }
        public DateTime? OrderMDate { get; set; }

        public StatusEnum OrderMStatus { get; set; }    
        public int OrderMAddressId { get; set; }
        public List<CustomerModel> CustomerVMM { get; set; }
        public List<AddressModel> AddressVMM { get; set; }

        public List<OrderDetailsFormViewModel> OrderDetailsMModels { get; set; }    

        public EditOrderOrderDetailsViewModel()               
        {                                                     
            OrderDetailsMModels = new List<OrderDetailsFormViewModel>();                     
        }
        public enum StatusEnum : byte  
        {
            [Display(Name = "Yeni Sipariş")]
            Yeni_Siparis = 1,
            [Display(Name = "Onaylandı")]
            Onaylandi = 2,
            [Display(Name = "Kargolandı")]
            Kargolandi = 3,
            [Display(Name = "Teslim Edildi")]
            Teslim_edildi = 4,
            [Display(Name = "İptal Edildi")]
            Iptal_edildi = 5,
        }
    }


}

