using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopBackEnd.Data.Entity
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = "";  


        public ICollection<Product> Products { get; set; } = new List<Product>();

        [Required]
        public DateTime LastModifiedDate { get; set; }  

    }
}
