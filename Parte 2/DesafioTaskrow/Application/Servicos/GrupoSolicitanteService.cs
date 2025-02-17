using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Exceptions;
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
    
    public async Task<List<GrupoSolicitanteRetorno>> ObterGruposSolicitantes(string? nome)
    {
        var query = _contexto.GruposSolicitantes.Where(x => true);
        
        if (nome is not null)
        {
            nome = nome.Trim().ToUpper();
            query = query.Where(x => x.Nome.ToUpper() == nome);
        }
        
        var retorno = await query.Select(x => new GrupoSolicitanteRetorno
        {
            Id = x.Id,
            Nome = x.Nome,
            GrupoPaiId = x.GrupoSolicitantePaiId
        }).ToListAsync();
        
        return retorno;
    }

    public async Task<GrupoSolicitante?> ObterPorId(Guid id)
    {
        var grupo = await _contexto.GruposSolicitantes.FindAsync(id);

        return grupo;
    }

    public Task<GrupoSolicitante>? ObterGruposSolicitantesFilhos(Guid grupoPaiId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CriarGrupoSolicitante(GrupoSolicitanteDto grupoSolicitanteDto)
    {
        if (string.IsNullOrWhiteSpace(grupoSolicitanteDto.Nome))
        {
            throw new NomeObrigatorioException("O nome do grupo solicitante é obrigatório.");
        }
        
        int nivelHierarquia = 1;
        
        if(grupoSolicitanteDto.GrupoPaiId is not null) 
        {
            var grupoPai = await _contexto.GruposSolicitantes.FindAsync(grupoSolicitanteDto.GrupoPaiId);

            if (grupoPai is null)
            {
                throw new GrupoPaiNaoEncontradoException("O grupo pai informado não existe.");
            }
            nivelHierarquia = await CalcularNivelHierarquia(grupoPai.Id);

            if (nivelHierarquia >= 5)
            {
                throw new HierarquiaMaximaException("O grupo pai informado não existe.");
            }
        }

        var novoGrupo = new GrupoSolicitante
        {
            Nome = grupoSolicitanteDto.Nome,
            GrupoSolicitantePaiId = grupoSolicitanteDto.GrupoPaiId,
        };
        
        _contexto.GruposSolicitantes.Add(novoGrupo);
        await _contexto.SaveChangesAsync();
        
        return novoGrupo.Id;
    }

    public Task RemoverGrupoSolicitante(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CalcularNivelHierarquia(Guid grupoPaiId)
    {
        int nivel = 1;
        var grupoAtual = await _contexto.GruposSolicitantes.FirstOrDefaultAsync(x => x.Id == grupoPaiId);

        while (grupoAtual?.GruposSolicitantePai != null)
        {
            nivel++;
            grupoAtual = await _contexto.GruposSolicitantes.FirstOrDefaultAsync(g => g.Id == grupoAtual.GrupoSolicitantePaiId);
        }

        return nivel;
    }
}