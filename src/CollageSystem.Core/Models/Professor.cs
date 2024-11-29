using System.ComponentModel.DataAnnotations.Schema;


namespace CollageSystem.Core.Models
{
    public class Professor : Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime HireDate { get; set; }

        [Column(TypeName = "decimal(12,2)")] public decimal Salary { get; set; }

        public ICollection<Course>? Courses { get; set; }
        public Department? Department { get; set; }
        public string? DepartmentName => Department?.Name;
        public int? DepartmentId { get; set; }
        public int CoursesCount { get; }
    }
}