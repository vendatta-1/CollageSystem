using System.ComponentModel.DataAnnotations;
using api.Helpers.CustomAttributes;
using CollageSystem.Core.Enums;
using CollageSystem.Utilities.Helpers.CustomAttributes;

namespace CollageSystem.Application.DTOs.Student;


public class StudentCreateDto : StudentBaseDto
{
    [Range(18, 26)] public int Age { get; set; }

    [UniqueEmail] public string Email { get; set; }

    [UniquePhoneNumber] public string PhoneNumber { get; set; }

    public int DepartmentId { get; set; }

    [Required] public AcademicYear AcademicYear { get; set; }
}