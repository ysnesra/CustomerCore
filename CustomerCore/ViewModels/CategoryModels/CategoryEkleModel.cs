using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{
    public class CategoryEkleModel
    {
        public int Id { get; set; }

        [Display(Name="Referans Id")]
        public int? ParentId { get; set; }

        [Display(Name ="Kategori")]
        public string Category { get; set; }

        [Display(Name = "En Alt Kategori Mi")]
        public bool? IsTheMostChild { get; set; }
    }
}
