using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{
    public class CustomerEkleModel
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı zorunludur.")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre girilmesi zorunludur.")]
        public string Password { get; set; }

        [Display(Name = "Ad-Soyad")]
        [Required(ErrorMessage = "Ad Soyad bilgileri zorunludur.")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Doğum günü girilmesi zorunludur.")]
        [Display(Name = "Doğum Günü")]
        [WithinTwoHundredYears]       
        public DateTime? Birthdate { get; set; }

        public string Email { get; set; }
        [Display(Name = "Cinsiyet")]
        public bool? Gender { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon bilgisi girilmelidir.")]
        public string Phone { get; set; }
    }
}

