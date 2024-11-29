
using AutoMapper;
using CollageSystem.Application.DTOs.Department;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Core.Interfaces.Repository;
using CollageSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace CollageSystem.Application.Services;

public class DepartmentService(
    ILogger<Service<Department, DepartmentBaseDto>> logger,
    IBaseRepository<Department> repository,
    IMapper mapper)
    : Service<Department, DepartmentBaseDto>(logger, repository, mapper), IDepartmentService
{
}