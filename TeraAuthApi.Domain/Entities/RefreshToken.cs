using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
        
    [Required]
    [StringLength(500)]
    public string Token { get; set; }
        
    [Required]
    [StringLength(50)]
    public string JwtId { get; set; }
        
    [Required]
    public DateTime CreatedDate { get; set; }
        
    [Required]
    public DateTime ExpirationDate { get; set; }
        
    [Required]
    public bool Invalidated { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public User User { get; set; }
    
    public void InvalidateToken()
    {
        Invalidated = true;
    }

}