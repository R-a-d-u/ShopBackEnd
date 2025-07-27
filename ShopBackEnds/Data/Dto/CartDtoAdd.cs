using System.ComponentModel.DataAnnotations;

namespace ShopBackEnd.Data.Dto
{
    public class CartDtoAdd
    {
        public int UserId { get; set; }
        public List<CartItemDto>? Items { get; set; } 
    }
}
