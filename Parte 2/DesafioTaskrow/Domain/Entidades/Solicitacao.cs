using DesafioTaskrow.Domain.Enums;

namespace DesafioTaskrow.Domain.Entidades;

public class Solicitacao : EntidadeBase
{
    public Guid GrupoSolicitanteId { get; set; }
    public Guid TipoSolicitacaoId { get; set; }
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
    public DateTime DataConclusao { get; set; }
    public EnumStatus Status { get; set; }
    public string Descricao { get; set; }
}