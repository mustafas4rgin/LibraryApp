namespace LibraryApp.Application.Results;

public class ErrorResult : ServiceResult
{
    public ErrorResult(string message) : base(false,message){}
}