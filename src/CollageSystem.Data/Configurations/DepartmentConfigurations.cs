
using CollageSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollageSystem.Data.Configurations;


public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // builder.Property(x=>x.CoursesCount)
        // .HasComputedColumnSql("SELECT COUNT(*) FROM Courses as s WHERE s.departmentId = Id",stored:true);
        //
        // builder.Property(x=>x.ExamsCount)
        // .HasComputedColumnSql("SELECT COUNT(*) FROM Exams as e WHERE e.departmentId = Id",stored:true);
        //
        // builder.Property(x=>x.ProfessorsCount)
        // .HasComputedColumnSql("SELECT COUNT(*) FROM Person as p WHERE p.departmentId = Id AND Type='Professor'",stored:true);
        //
        // builder.Property(x=>x.StudentsCount)
        // .HasComputedColumnSql("SELECT COUNT(*) FROM Person as s WHERE s.departmentId = Id AND Type='Student'",stored:true);
        builder.Property(x => x.CoursesCount)
            .HasColumnName("CoursesCount");
        builder.Property(x => x.ExamsCount)
            .HasColumnName("ExamsCount");
        builder.Property(x => x.ProfessorsCount)
            .HasColumnName("ProfessorsCount");
        builder.Property(x => x.StudentsCount)
            .HasColumnName("StudentsCount");
    }
}