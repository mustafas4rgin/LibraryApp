using LibraryApp.Application.Concrete;

namespace LibraryApp.Application.Results;

public class ServiceResult : IServiceResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;

    public ServiceResult(bool success,string message)
    {
        Success = success;
        Message = message;
    }
}
