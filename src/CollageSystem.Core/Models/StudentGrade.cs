using System.ComponentModel.DataAnnotations.Schema;

using CollageSystem.Core.Enums;

namespace CollageSystem.Core.Models
{
    public class StudentGrade
    {
        public StudentGrade(Student student)
        {
            Student = student;
        }

        public StudentGrade() : this(new Student())
        {
        }

        [ForeignKey(nameof(Student))] public int StudentId { get; set; }

        public Student? Student { get; set; }
        public Grade? Grade { get; set; }
        public int GradeId { get; set; }
        public AcademicYear GradeLevel { get; set; }
    }
}