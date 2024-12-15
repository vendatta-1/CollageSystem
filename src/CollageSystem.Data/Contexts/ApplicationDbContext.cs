using CollageSystem.Core.Interfaces.Data;
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.RelationshipEntities;
using CollageSystem.Core.Models.SecurityModels;
using CollageSystem.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CollageSystem.Data.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfigurations());
            modelBuilder.ApplyConfiguration(new CourseConfigurations());
            modelBuilder.ApplyConfiguration(new StudentConfigurations());

            modelBuilder.Entity<Person>()
                .HasDiscriminator<string>("Type")
                .HasValue<Student>("Student")
                .HasValue<Professor>("Professor");


            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Professors)
                .HasForeignKey(p => p.DepartmentId);

            modelBuilder.Entity<Professor>()
                .Property(x => x.HireDate)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<StudentCourse>()
                .HasOne(c => c.Student)
                .WithMany(c => c.Courses);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(x => new { x.StudentId, x.CourseId });


            modelBuilder.Entity<StudentCourse>()
                .HasOne(c => c.Course)
                .WithMany(c => c.Students)
                ;
            modelBuilder.Entity<StudentCourse>()
                .ToTable("StudentCourse");


            modelBuilder.Entity<StudentGrade>()
                .HasKey(x => new { x.StudentId, x.GradeId });


            modelBuilder.Entity<StudentGrade>()
                .HasOne(x => x.Student)
                .WithMany(x => x.Grades);

            modelBuilder.Entity<Person>()
                .HasOne<Address>()
                .WithOne(x => x.Person)
                .HasForeignKey<Address>(x => x.PersonId);

            modelBuilder.Entity<Person>()
                .ToTable(t =>
                {
                    t.HasTrigger("trg_AutoAssignCoursesToNewStudent");
                    t.HasTrigger("trg_UpdateCourseAndStudentCountsUpdate");
                    t.HasTrigger("trg_UpdateDepartmentStudentsCountDelete");
                    t.HasTrigger("trg_UpdateDepartmentStudentsCountInsert");
                });

            modelBuilder.Entity<Course>()
                .ToTable(t =>
                {
                    t.HasTrigger("trg_UpdateDepartmentCoursesCount");
                    t.HasTrigger("trg_UpdateProfessorCoursesCount");
                });

            modelBuilder.Entity<StudentCourse>()
                .ToTable(t => { t.HasTrigger("trg_UpdateCourseAndStudentCounts"); });


            modelBuilder.Entity<Address>()
                .HasOne(x => x.Person)
                .WithOne(x => x.Address)
                .HasForeignKey<Person>(x => x.AddressId);

            modelBuilder.Entity<Address>()
                .ToTable("Addresses");
        }

        #region DbSets


        //public virtual DbSet<Student> Students { get; set; }
        //public virtual DbSet<Professor> Professors { get; set; }
        //public virtual DbSet<Department> Departments { get; set; }
        //public virtual DbSet<Course> Courses { get; set; }
        //public virtual DbSet<Exam> Exams { get; set; }
        //public virtual DbSet<Grade> Grades { get; set; }
        //public virtual DbSet<StudentCourse> StudentCourses { get; set; }
        //public virtual DbSet<StudentGrade> StudentGrades { get; set; }
        //public virtual DbSet<StudentCrucialInformation> StudentCrucialInformation { get; set; }

        #endregion
    }
}