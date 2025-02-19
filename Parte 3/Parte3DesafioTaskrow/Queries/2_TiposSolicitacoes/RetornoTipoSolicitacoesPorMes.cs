namespace Parte3DesafioTaskrow.Queries._2_TiposSolicitacoes;

public class RetornoTipoSolicitacoesPorMes
{
    public Guid TipoSolicitacaoId { get; set; }
    public string NomeTipoSolicitacao { get; set; }
    public int TotalSolicitacoes { get; set; }
    public Guid? GrupoSolicitanteId { get; set; }
    public string NomeGrupoSolicitante { get; set; }
    public int TotalSolicitacoesGrupoSolicitante { get; set; }
}