namespace DesafioTaskrow.Domain.Exceptions;

public class LimiteJaExisteException : Exception
{
    public LimiteJaExisteException(string message) : base(message)
    {
    }
}