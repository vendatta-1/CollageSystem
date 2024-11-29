using System.Text.Json.Serialization;

using CollageSystem.Application.DTOs.Department;

namespace CollageSystem.Application.DTOs.Student;

public class StudentResponseDto : StudentBaseDto
{
    public int Age { get; set; }
    public string Name { get; set; }

    [JsonIgnore] public DepartmentResponseDto? Department { get; set; }

    public string? DepartmentName => Department?.Name;
    public string StudentCode { get; set; }
    public double TotalQualityPoints { get; set; }
    public double TotalCreditHours { get; set; }
}