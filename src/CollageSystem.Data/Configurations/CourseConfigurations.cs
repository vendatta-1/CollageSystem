
using CollageSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollageSystem.Data.Configurations;

public class CourseConfigurations : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        //     builder.Property(x=>x.StudentCount)
        //         .HasComputedColumnSql("SELECT COUNT(*) FROM [StudentCourse] as SC WHERE SC.CourseId=Id",stored:true);
        builder.Property(x => x.StudentsCount)
            .HasColumnName("StudentsCount");

        builder
            .HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId);
    }
}