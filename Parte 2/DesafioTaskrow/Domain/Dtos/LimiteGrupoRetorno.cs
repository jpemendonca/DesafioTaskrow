namespace DesafioTaskrow.Domain.Dtos;

public class LimiteGrupoRetorno
{
    public Guid GrupoSolicitanteId { get; set; }
    public Guid TipoSolicitacaoId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int LimiteMensal { get; set; }
    public Guid Id { get; set; }
}