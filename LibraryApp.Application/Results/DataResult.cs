using LibraryApp.Application.Concrete;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Results;

public class DataResult<T> : IServiceResult<T> where T : class
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; } = null!;

    public DataResult(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}