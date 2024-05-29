using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Domain.Entities;

public class UserRole
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
        
    [Required]
    public User User { get; set; }

    [Required]
    public Guid RoleId { get; set; }
        
    [Required]
    public Role Role { get; set; }
}