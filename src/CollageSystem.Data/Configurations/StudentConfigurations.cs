
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.SecurityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CollageSystem.Data.Configurations;


public class StudentConfigurations : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasOne(s => s.Department)
            .WithMany(d => d.Students)
            .HasForeignKey(s => s.DepartmentId);

        // builder.Property(x=>x.CourseCount)
        //     .HasComputedColumnSql("SELECT COUNT(*) FROM [StudentCourse] WHERE StudentCourse.studentId = Id ",stored:true);

        builder.Property(x => x.CoursesCount)
            .HasColumnName("CoursesCount");
        builder.Property(s => s.StudentCode)
            .IsRequired();

        builder.HasOne(s => s.StudentCrucialInformation)
            .WithOne(sc => sc.Student)
            .HasForeignKey<StudentCrucialInformation>(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        // builder
        //     .Property(p => p.CoursesCount)
        //     .ValueGeneratedOnAddOrUpdate()
        //     .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}