namespace CollageSystem.Core.Models
{
    public class Exam : BaseEntity
    {
        //private const int MAX_GRADE=100;
        public int MaxGrade { get; set; }
        public DateTime? Created { get; set; }
        public float Duration { get; set; }
        public Department Department { get; set; } = new();
        public int? DepartmentId { get; set; }

        public string? DepartmentName => Department.Name;
    }
}