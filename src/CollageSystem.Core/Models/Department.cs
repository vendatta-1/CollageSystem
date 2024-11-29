namespace CollageSystem.Core.Models
{
    public class Department : BaseEntity
    {
        private const int DefaultMaxStudentCount = 100;

        private int _maxStudentCount;
        public int ProfessorsCount { get; init; }

        public int StudentsCount { get; init; }

        public int CoursesCount { get; init; }

        public int ExamsCount { get; init; }

        public int MaxStudentCount
        {
            get => _maxStudentCount;
            set => _maxStudentCount = value <= 25 ? DefaultMaxStudentCount : value;
        }

        public ICollection<Professor>? Professors { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Exam>? Exams { get; set; }
    }
}