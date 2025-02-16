using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class GrupoSolicitanteService : IGrupoSolicitanteService
{
    private readonly Contexto _contexto;

    public GrupoSolicitanteService(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<List<GrupoSolicitante>> ObterGruposSolicitantes(string? nome)
    {
        var query = _contexto.GruposSolicitantes.Where(x => true);
        
        if (nome is not null)
        {
            nome = nome.Trim().ToUpper();
            query = query.Where(x => x.Nome.ToUpper() == nome);
        }

        return await query.ToListAsync();
    }

    public Task<GrupoSolicitante>? ObterGrupoSolicitante(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<GrupoSolicitante>? ObterGruposSolicitantesFilhos(Guid grupoPaiId)
    {
        throw new NotImplementedException();
    }

    public Task RemoverGrupoSolicitante(Guid id)
    {
        throw new NotImplementedException();
    }
}