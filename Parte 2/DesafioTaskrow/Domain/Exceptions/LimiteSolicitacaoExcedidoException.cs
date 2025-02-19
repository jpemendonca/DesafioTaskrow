namespace DesafioTaskrow.Domain.Exceptions;

public class LimiteSolicitacaoExcedidoException : Exception
{
    public LimiteSolicitacaoExcedidoException(string message) : base(message)
    {
    }
}