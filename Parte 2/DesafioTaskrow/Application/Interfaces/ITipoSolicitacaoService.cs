using DesafioTaskrow.Domain.Dtos;

namespace DesafioTaskrow.Application.Interfaces;

public interface ITipoSolicitacaoService
{
    Task<List<TipoSolicitacaoRetorno>> ObterTiposSolicitacao();
    Task<Guid> CriarTiposSolicitacao(string nome);
    Task EditarTiposSolicitacao(Guid id, string nome, bool ativo);
    Task ExcluirTiposSolicitacao(Guid id);
}