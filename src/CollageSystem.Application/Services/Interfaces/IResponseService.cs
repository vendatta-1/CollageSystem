
using CollageSystem.Application.DTOs;

namespace CollageSystem.Application.Services.Interfaces;

public interface IResponseService
{
    Task<Response<T>> HandleResponse<T>(string? message, bool success, T data, params string[] errors)
        where T : class, new();
}