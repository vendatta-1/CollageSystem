using System.ComponentModel.DataAnnotations;

using CollageSystem.Core.Enums;
using CollageSystem.Core.Models.RelationshipEntities;
using CollageSystem.Core.Models.SecurityModels;

namespace CollageSystem.Core.Models
{
    public class Student : Person
    {
        public ICollection<StudentCourse>? Courses { get; set; }
        public ICollection<StudentGrade>? Grades { get; set; }
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        public int? CoursesCount { get; init; }
        public string? DepartmentName => Department?.Name;

        public double TotalQualityPoints { get; set; }
        public double TotalCreditHours { get; set; }

        [MaxLength(12)] public string StudentCode { get; set; }

        public DateOnly BirthDate { get; set; }
        public DateTime JoinTime { get; set; } = DateTime.Now;
        public StudentCrucialInformation StudentCrucialInformation { get; set; }

        public AcademicYear? AcademicYear { get; set; }
    }
}