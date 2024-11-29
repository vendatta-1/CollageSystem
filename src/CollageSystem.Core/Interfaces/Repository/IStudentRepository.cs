using CollageSystem.Core.Models;
using CollageSystem.Core.Results;

namespace CollageSystem.Core.Interfaces.Repository;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<OperationResult> CreateStudentInfo(Student student);
}