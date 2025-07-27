using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Entity
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = "";

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalValue { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal GoldWeightInGrams { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; } = 0;

        [Required]
        public int StockQuantity { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public ProductState ProductState { get; set; }

        public string Description { get; set; } = "";
        public string? Image { get; set; }

        public Category Category { get; set; }


        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
