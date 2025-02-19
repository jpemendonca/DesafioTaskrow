using DesafioTaskrow.Application.Dtos;

namespace DesafioTaskrow.Application.Interfaces;

public interface IVerificacaoSolicitacaoService
{
    Task VerificarSolicitacao(SolicitacaoDto solicitacao);
    Task VerificarGrupoSolicitante(Guid grupoId);
    Task VerificarTipoSolicitacao(Guid tipoId);
    Task VerificarLimiteMensal(Guid grupoId);
}