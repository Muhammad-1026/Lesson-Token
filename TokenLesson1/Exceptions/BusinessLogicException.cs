namespace TokenLesson1.Exeptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException()
    {
    }

    public BusinessLogicException(string message)
        : base(message)
    {
    }

    public BusinessLogicException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
