using CollageSystem.Application.DTOs.Student;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Core.Models;
using CollageSystem.Core.Results;


namespace CollageSystem.Application.Services.Interfaces;


public interface IStudentService : IService<Student, StudentBaseDto>
{
    Task<OperationResult> Update(string studentCode, StudentUpdateDto student);
    Task<OperationResult> Delete(string studentCode);
}