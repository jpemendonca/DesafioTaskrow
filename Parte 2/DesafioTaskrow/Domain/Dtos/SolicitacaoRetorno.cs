using DesafioTaskrow.Domain.Enums;

namespace DesafioTaskrow.Domain.Dtos;

public class SolicitacaoRetorno
{
    public Guid Id { get; set; }
    public Guid GrupoSolicitanteId { get; set; }
    public Guid TipoSolicitacaoId { get; set; }
    public DateTime DataConclusao { get; set; }
    public EnumStatus Status { get; set; }
    public string Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
}