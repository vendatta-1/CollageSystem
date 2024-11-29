using System.ComponentModel.DataAnnotations;

namespace CollageSystem.Core.Models
{
    public abstract class BaseEntity
    {
        [Required] public int Id { get; set; }

        [Required, MaxLength(70)] public string Name { get; set; } = string.Empty;
    }
}