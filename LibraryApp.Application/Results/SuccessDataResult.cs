
namespace LibraryApp.Application.Results;

public class SuccessDataResult<T> : DataResult<T> where T : class
{
    public SuccessDataResult(string message, T data) : base(true,message,data){}
}