namespace LibraryApp.Application.Concrete;

public interface IServiceResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
public interface IServiceResult<T> : IServiceResult
{
    public T Data { get; set; }
}