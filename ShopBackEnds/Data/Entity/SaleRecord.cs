using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopBackEnd.Data.Entity
{
    public class SaleRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        [ForeignKey("OrderItemId")]
        public OrderItem OrderItem { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }
    }
}