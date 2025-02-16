using DesafioTaskrow.Domain.Models;

namespace DesafioTaskrow.Application.Interfaces;

public interface IGrupoSolicitanteService
{
    Task<List<GrupoSolicitante>> ObterGruposSolicitantes(string? nome);
    Task<GrupoSolicitante>? ObterGrupoSolicitante(Guid id);
    Task<GrupoSolicitante>? ObterGruposSolicitantesFilhos(Guid grupoPaiId);
    // Task CriarGrupoSolicitante(GrupoSolicitante g);
    // Task EditarGrupoSolicitante(GrupoSolicitante g);
    Task RemoverGrupoSolicitante(Guid id);
}