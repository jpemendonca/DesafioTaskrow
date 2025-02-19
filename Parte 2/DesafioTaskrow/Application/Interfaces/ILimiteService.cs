using DesafioTaskrow.Application.Dtos;

namespace DesafioTaskrow.Application.Interfaces;

public interface ILimiteService
{
    Task<List<LimiteGrupoRetorno>> ObterLimitesGrupos();
    Task<Guid> CriarLimiteGrupo(LimiteGrupoDto limiteGrupo);
    Task EditarLimiteGrupo(Guid id, LimiteGrupoDto limiteGrupo);
    Task RemoverLimiteGrupo(Guid id);
    Task<List<LimiteGrupoRetorno>>  ObterLimiteGrupoPorTipoSolicitacaoMesEspecifico(Guid idGrupo, Guid idTipo, int ano, int mes);
}