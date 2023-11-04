public class ErrorMsg
{
    public int Code { get; set; }
    public string Message { get; set; }

    public ErrorMsg(int code, string message)
    {
        Code = code;
        Message = message;
    }
}