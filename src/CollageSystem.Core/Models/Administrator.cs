using CollageSystem.Core.Enums;

namespace CollageSystem.Core.Models
{
    public class Administrator : Person
    {
        public int Age { get; set; }


        public AdminPosition AdminPosition { get; set; }
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName => Department?.Name;
    }
}