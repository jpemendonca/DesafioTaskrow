namespace DesafioTaskrow.Domain.Entidades;

public class TipoSolicitacao : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}