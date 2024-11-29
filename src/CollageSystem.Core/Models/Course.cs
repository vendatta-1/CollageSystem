using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CollageSystem.Core.Enums;
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.RelationshipEntities;

namespace CollageSystem.Core.Models
{
    public class Course : BaseEntity
    {
        private int _semester;
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Professor? Professor { get; set; }

        public string? DepartmentName => Department?.Name;
        public AcademicYear Year { get; set; }

        [MaxLength(20)] public string CourseCode { get; set; } = string.Empty;

        public int? ProfessorId { get; set; }
        public int? DepartmentId { get; set; }

        public int Semester
        {
            get => _semester;
            set
            {
                if (value is <= 0 or > 2)
                    throw new ArgumentException("can not the semester exceeded three ");
                _semester = value;
            }
        }


        public int StudentsCount { get; set; }

        public virtual Department? Department { get; set; }

        public virtual IEnumerable<StudentCourse>? Students { get; set; }
    }
}