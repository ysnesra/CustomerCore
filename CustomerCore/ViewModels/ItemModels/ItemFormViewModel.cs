
using CustomerCore.Context;
using CustomerCore.ViewModels.PhotoModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerCore.ViewModels
{
    public class ItemFormViewModel
    {
        public int ItmId { get; set; }

        [Display(Name = "Ürün Kodu")]
        public string ItmItemCode { get; set; }

        [Display(Name = "Ürün")]
        public string ItmItem { get; set; }

        [Display(Name = "Birim Fiyatı")]
        public decimal ItmUnitPrice { get; set; }

        [Display(Name = "Markası")]
        public string ItmBrand { get; set; }

        [Display(Name = "Miktarı")]
        public int ItmAmount { get; set; }

        [Display(Name = "Fotograf Seç")]
        [NotMapped]  
        public List<IFormFile> ItmSelectFiles { get; set; } 

        [Display(Name = "Resim")]
        public List<PhotoModel> PhotoFiles { get; set; } = new List<PhotoModel>();  
       
        [Display(Name = "Kategori Id")]
        public int ItmCategoryId { get; set; }

        [Display(Name = "Kdv Id")]
        public int ItmVatRateId { get; set; }
        public List<VatRateModel> VatRateVM { get; set; }
        public List<CategoryModel> CategoryVM { get; set; }
    }
}
