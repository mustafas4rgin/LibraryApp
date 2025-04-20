using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Results;

public class ErrorDataResult<T> : DataResult<T> where T : class
{
    public ErrorDataResult(string message) : base(false,message,default!){}
}