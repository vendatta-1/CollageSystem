namespace CollageSystem.Core.Models
{
    public class Grade : BaseEntity
    {
        public int ExamId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public Student? Student { get; set; }
        public Exam? Exam { get; set; }
        public string ExamName => Exam?.Name ?? " ";

        public string StudentName => Student?.Name ?? "";
        public double ExamGrade { get; set; }
    }
}