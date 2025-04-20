using LibraryApp.Application.Results;

namespace LibraryApp.Application.Results;

public class SuccessResult : ServiceResult
{
    public SuccessResult(string message) : base(true,message){}
}