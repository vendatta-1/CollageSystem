using CollageSystem.Core.Models.RelationshipEntities;
using CollageSystem.Core.Models.SecurityModels;
using CollageSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace CollageSystem.Core.Interfaces.Data
{
    public interface IApplicationDbContext 
    {
        public  DbSet<Student> Students { get; }
        public  DbSet<Professor> Professors { get; }
        public  DbSet<Department> Departments { get;  }
        public  DbSet<Course> Courses { get; }
        public  DbSet<Exam> Exams { get; }
        public  DbSet<Grade> Grades { get; }
        public  DbSet<StudentCourse> StudentCourses { get; }
        public  DbSet<StudentGrade> StudentGrades { get;  }
        public  DbSet<StudentCrucialInformation> StudentCrucialInformation { get; }
    }
}
