namespace ShopBackEnd.Data.Dto
{
    public class UserDtoEdit
    {
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime LastModifyDate { get; set; } = DateTime.Now;
    }
}
