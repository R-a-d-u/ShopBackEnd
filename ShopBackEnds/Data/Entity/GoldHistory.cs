using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopBackEnd.Data.Entity
{
    public class GoldHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Metal { get; set; } = "";
        [Required]
        public decimal PriceOunce { get; set; }
        [Required]
        public decimal PriceGram { get; set; }
        [Required]
        public double PercentageChange { get; set; }
        [Required]
        [MaxLength(255)]
        public string Exchange { get; set; } = "";
        [Required]
        [MaxLength(255)]
        public string Timestamp { get; set; } = "";
        [Required]
        [MaxLength(255)]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
