public class APIException : Exception
{
    public readonly int Code;
    public readonly int Status;

    public APIException(
        int code,
        int status,
        string message
    ) : base(message)
    {
        Code = code;
        Status = status;
    }
}