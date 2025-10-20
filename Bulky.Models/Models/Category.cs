using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set;}
        [Required]
        [DisplayName("Category Name")]    //added data custome annotation for UI bind
        [MaxLength(30)]                   //Name validation
        public string Name { get; set; }
       
        [DisplayName("Dispay Order")]     //added data custome annotation for UI bind 
        [Range(1,100, ErrorMessage ="Display Order must be between 1-100")]     //Order validation and custome error message
        public int DisplayOrder { get; set; }
    }
}
