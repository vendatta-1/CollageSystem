using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollageSystem.Core.Models.SecurityModels;
public class StudentCrucialInformation
{
    [Key] 
    public string StudentCode { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))] 
    public Student Student { get; set; }

    public string UniversityEmail { get; set; }
    public string Password { get; set; }
}