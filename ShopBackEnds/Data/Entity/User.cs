using ShopBackEnd.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = "";

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = "";

    [Required]
    [MaxLength(255)]
    public string PhoneNumber { get; set; } = "";

    [Required]
    [MaxLength(50)]
    public UserAccesType UserAccessType { get; set; }

    [Required]
    public DateTime LastModifyDate { get; set; }
    public DateTime CreationDate { get; set; }
    [Required]
    public bool IsDeleted { get; set; }

    public bool EmailConfirmed { get; set; } = false;
    [MaxLength(512)]
    public string? EmailConfirmationToken { get; set; }
    public DateTime? EmailConfirmationTokenExpiry { get; set; }
    [MaxLength(512)]
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }
}
