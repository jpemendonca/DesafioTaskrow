namespace DesafioTaskrow.Domain.Exceptions;

public class GrupoNaoEncontradoException : Exception
{
    public GrupoNaoEncontradoException(string message) : base(message)
    {
    }
}