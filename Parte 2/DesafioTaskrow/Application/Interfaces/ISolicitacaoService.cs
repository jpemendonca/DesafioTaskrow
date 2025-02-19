using DesafioTaskrow.Domain.Dtos;

namespace DesafioTaskrow.Application.Interfaces;

public interface ISolicitacaoService
{
    Task<List<SolicitacaoRetorno>> ObterSolicitacoes();
    Task<Guid> CriarSolicitacao(SolicitacaoDto solicitacao);
    Task RemoverSolicitacao(Guid id);
    Task EditarSolicitacao(Guid id, SolicitacaoDto solicitacao);
}