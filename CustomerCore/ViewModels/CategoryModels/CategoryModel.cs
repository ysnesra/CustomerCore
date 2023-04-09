using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; set; }     
        public int? ParentId { get; set; }
        public string Category { get; set; }
        public bool? IsTheMostChild { get; set; }
    }
}
