namespace DesafioTaskrow.Domain.Exceptions;

public class NomeObrigatorioException : Exception
{
    public NomeObrigatorioException(string message) : base(message)
    {
    }
}