using System.ComponentModel.DataAnnotations;

using AutoMapper;
using CollageSystem.Application.DTOs.Student;
using CollageSystem.Application.Services;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Core.Interfaces.Repository;
using CollageSystem.Core.Models;
using CollageSystem.Core.Results;
using CollageSystem.Core.Validation;
using Microsoft.Extensions.Logging;

namespace CollageSystem.Application.Services;

public class StudentService(
    ILogger<Service<Student, StudentBaseDto>> logger,
    IStudentRepository repository,
    IMapper mapper) : Service<Student, StudentBaseDto>(logger, repository, mapper), IStudentService
{
    private readonly OperationResult _result =
        new OperationResult(OperationStatus.Pending, new LoggerFactory().CreateLogger<OperationResult>());

    public override Task<OperationResult> Create(StudentBaseDto entity)
    {
        var student = mapper.Map<Student>(entity);


        return repository.Create(student);
    }

    public async Task<OperationResult> Update(string studentCode, StudentUpdateDto student)
    {
        try
        {
            var studentEntity = await repository.Get(x => x.StudentCode == studentCode);
            if (studentEntity is null)
                return _result.WithErrorCode(ErrorCode.NotFound);
            await base.Update(studentEntity.Id, student);

            return _result.WithStatus(OperationStatus.Success);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return _result.WithStatus(OperationStatus.Failure).WithException(e);
        }
    }

    public async Task<OperationResult> Delete(string studentCode)
    {
        try
        {
            var studentEntity = await repository.Get(x => x.StudentCode == studentCode);
            if (studentEntity is null)
                return _result.WithErrorCode(ErrorCode.NotFound);

            await repository.Delete(studentEntity.Id);
            return _result.WithStatus(OperationStatus.Success);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return _result.WithStatus(OperationStatus.Failure).WithException(e);
        }
    }
}