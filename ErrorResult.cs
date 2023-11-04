public class ErrorResult
{
    public int code { get; }
    public string message { get; }

    public ErrorResult(int code, string message)
    {
        this.code = code;
        this.message = message;
    }
}