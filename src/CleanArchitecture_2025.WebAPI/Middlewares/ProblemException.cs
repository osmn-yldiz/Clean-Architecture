namespace CleanArchitecture_2025.WebAPI.Middlewares;

[Serializable]
public class ProblemException : Exception
{
    public string Error { get; }

    public override string Message => base.Message;

    public ProblemException(string error, string message) : base(message)
    {
        Error = error;
    }
}
