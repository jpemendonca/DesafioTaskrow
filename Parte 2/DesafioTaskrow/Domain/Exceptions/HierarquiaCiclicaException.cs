namespace DesafioTaskrow.Domain.Exceptions;

public class HierarquiaCiclicaException : Exception
{
    public HierarquiaCiclicaException(string message) : base(message)
    {
    }
}