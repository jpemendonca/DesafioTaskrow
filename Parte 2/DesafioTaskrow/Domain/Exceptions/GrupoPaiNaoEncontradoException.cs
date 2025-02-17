namespace DesafioTaskrow.Domain.Exceptions;

public class GrupoPaiNaoEncontradoException : Exception
{
    public GrupoPaiNaoEncontradoException(string message) : base(message)
    {
    }
}