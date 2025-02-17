using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Models;

namespace DesafioTaskrow.Application.Interfaces;

public interface IGrupoSolicitanteService
{
    Task<List<GrupoSolicitanteRetorno>> ObterGruposSolicitantes(string? nome);
    Task<GrupoSolicitante?> ObterPorId(Guid id);
    Task<GrupoSolicitante>? ObterGruposSolicitantesFilhos(Guid grupoPaiId);
    Task<Guid> CriarGrupoSolicitante(GrupoSolicitanteDto grupoSolicitanteDto);
    // Task EditarGrupoSolicitante(GrupoSolicitante g);
    Task RemoverGrupoSolicitante(Guid id);
    Task<int> CalcularNivelHierarquia(Guid grupoPaiId);
}