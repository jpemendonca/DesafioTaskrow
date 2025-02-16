using DesafioTaskrow.Domain.Enums;

namespace DesafioTaskrow.Domain.Entidades;

public class Log : EntidadeBase
{
    public EnumLogLevel LogLevel { get; set; }
    public string Local { get; set; } = string.Empty;
    public string? Erro { get; set; }
    public string? Detalhes { get; set; }
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
}