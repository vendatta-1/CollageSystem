using System.ComponentModel.DataAnnotations;

namespace CollageSystem.Application.DTOs.Student;

public class StudentUpdateDto : StudentBaseDto
{
    [Required] public string StudentCode { get; set; }

    [Range(1, 5)] public int? DepartmentId { get; set; }

    [StringLength(maximumLength: 25, MinimumLength = 3)]
    public string? Name { get; set; }

    [MaxLength(14)] public string? PhoneNumber { get; set; }

    [EmailAddress] public string? Email { get; set; }
}