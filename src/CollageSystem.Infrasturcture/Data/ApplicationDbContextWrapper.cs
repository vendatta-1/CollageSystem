using CollageSystem.Core.Interfaces.Data;
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.RelationshipEntities;
using CollageSystem.Core.Models.SecurityModels;
using CollageSystem.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CollageSystem.Infrastructure.Data
{
    public class ApplicationDbContextWrapper(ApplicationDbContext context) : IApplicationDbContext
    {
        public DbSet<Student> Students => context.Students;
        public DbSet<Professor> Professors => context.Professors;
        public DbSet<Department> Departments => context.Departments;
        public DbSet<Course> Courses => context.Courses;
        public DbSet<Exam> Exams => context.Exams;
        public DbSet<Grade> Grades => context.Grades;
        public DbSet<StudentCourse> StudentCourses => context.StudentCourses;
        public DbSet<StudentGrade> StudentGrades => context.StudentGrades;
        public DbSet<StudentCrucialInformation> StudentCrucialInformation => context.StudentCrucialInformation;
    }
}
