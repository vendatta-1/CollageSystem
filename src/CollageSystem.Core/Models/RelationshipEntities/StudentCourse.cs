using System.ComponentModel.DataAnnotations.Schema;


namespace CollageSystem.Core.Models.RelationshipEntities
{
    public class StudentCourse
    {
        public StudentCourse()
        {
            Student = new Student();
            Course = new Course();
        }

        [ForeignKey(nameof(Student))] public int StudentId { get; set; }

        [ForeignKey(nameof(Course))] public int CourseId { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}