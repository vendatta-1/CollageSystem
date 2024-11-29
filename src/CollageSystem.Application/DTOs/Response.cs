namespace CollageSystem.Application.DTOs;

public class Response<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; }
}
