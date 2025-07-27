namespace ShopBackEnd.Data.Dto
{
    public class UserDtoEditPassword
    {
        public string Password { get; set; } = "";
        public DateTime LastModifyDate { get; set; } = DateTime.Now;
    }
}
