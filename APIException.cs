public class APIException : Exception
{
    public readonly int Status;

    public APIException(
        int status,
        string message
    ) : base(message)
    {
        Status = status;
    }
}