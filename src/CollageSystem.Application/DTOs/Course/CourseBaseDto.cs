namespace CollageSystem.Application.DTOs.Course;

public class CourseBaseDto
{
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string DepartmentName { get; set; }
}