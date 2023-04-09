using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels.AddressModels
{
    public class AddressFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Müşteri Id'si")]
        public int? CustomerId { get; set; }

        [Display(Name = "Ülke Id'si")]
        public byte? CountryId { get; set; }

        [Display(Name = "Şehir Id'si")]
        public short? CityId { get; set; }

        [Display(Name = "İlçe Id'si")]
        public int? TownId { get; set; }

        [Display(Name = "Semt Id'si")]
        public int? DistrictId { get; set; }

        [Display(Name = "Posta Kodu")]
        public string PostalCode { get; set; }

        [Display(Name = "Adres")]
        public string AddressText { get; set; }
        public List<CustomerModel> CustomerVM { get; set; }
        public List<CountryModel> CountryVM { get; set; }
        public List<CityModel> CityVM { get; set; }
        public List<TownModel> TownVM { get; set; }
        public List<DistrictModel> DistrictVM { get; set; }
    }
}
