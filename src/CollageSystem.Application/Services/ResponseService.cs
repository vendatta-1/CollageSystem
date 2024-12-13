
using CollageSystem.Application.DTOs;
using CollageSystem.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CollageSystem.Application.Services;

public class ResponseService(ILogger<ResponseService> logger) : IResponseService
{
    private ILogger<ResponseService> Logger{get;  } = logger;

    public Task<Response<T>> HandleResponse<T>(string? message, bool success, T data, params string[] errors)where T:class,new()
    {
        try
        {
            var response = new Response<T>
            {
                Message = message ?? " ",
                IsSuccess = success,
                Data = new T(),
                Errors = errors.ToList()
            };
            return Task.FromResult(Task.FromResult(response).Result);
        }
        catch (Exception e)
        {
           Logger.LogError(e.Message);
            errors.ToList().Add(e.Message);
                
            return Task.FromResult(new Response<T>()
            {
                Errors =errors.ToList()
            });
        }
    }
}