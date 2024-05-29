using System.ComponentModel.DataAnnotations;

namespace TeraAuthApi.Domain.Entities;

public class Role
{
    [Key]
    public Guid Id { get; set; }
        
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
}