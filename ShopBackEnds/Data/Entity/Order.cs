using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShopBackEnd.Data.Enums;

namespace ShopBackEnd.Data.Entity
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderCreatedDate { get; set; }

        public DateTime? OrderCompletedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSum { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        [Required]
        public string Address { get; set; } = "";

        [Required]
        [MaxLength(50)]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
