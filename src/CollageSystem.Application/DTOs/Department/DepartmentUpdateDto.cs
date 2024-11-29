using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CollageSystem.Application.DTOs.Department;

public record class DepartmentUpdateDto : DepartmentBaseDto
{
    [Required] public int Id { get; set; }

    public int? MaxStudentCount { get; set; }
}