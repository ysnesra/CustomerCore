using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string NameSurname { get; set; }

        public DateTime Birthdate { get; set; }

        [Required]
        [Display(Name = "Oluşturulma Zamanı")]
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        [Display(Name = "Cinsiyet")]
        public bool? Gender { get; set; }
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
    }
}

