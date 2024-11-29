
using CollageSystem.Application.DTOs.Course;
using CollageSystem.Application.DTOs.Exam;
using CollageSystem.Application.DTOs.Professor;

namespace CollageSystem.Application.DTOs.Department;

public record class DepartmentResponseDto : DepartmentBaseDto
{
    public int ProfessorsCount { get; init; }

    public int StudentsCount { get; init; }

    public int CoursesCount { get; init; }

    public int ExamsCount { get; init; }
    public int MaxStudentCount { get; set; }
    public IEnumerable<ProfessorBaseDto>? Professors { get; set; }
    public IEnumerable<ExamBaseDto>? Exams { get; set; }
    public IEnumerable<CourseBaseDto>? Courses { get; set; }
}