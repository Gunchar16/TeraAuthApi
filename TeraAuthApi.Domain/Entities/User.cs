using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Domain.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
        
    [Required]
    [StringLength(50)]
    public string Username { get; set; }
        
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; }
        
    [Required]
    public DateTime CreatedAt { get; set; }
        
    public DateTime? UpdatedAt { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}