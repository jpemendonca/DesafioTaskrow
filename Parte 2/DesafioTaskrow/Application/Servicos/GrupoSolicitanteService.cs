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
    private readonly IVerificacaoGrupoSolicitanteService _verificacaoService;

    public GrupoSolicitanteService(Contexto contexto, IVerificacaoGrupoSolicitanteService verificacaoService)
    {
        _contexto = contexto;
        _verificacaoService = verificacaoService;
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
            
            // if (await _verificacaoService.ExisteCicloNaHierarquia(grupoSolicitanteDto.GrupoPaiId.Value))
            // {
            //     throw new HierarquiaCiclicaException("A hierarquia de grupos não pode conter ciclos.");
            // }
            
            nivelHierarquia = await _verificacaoService.CalcularNivelHierarquia(grupoPai.Id);

            if (nivelHierarquia >= 5)
            {
                throw new HierarquiaMaximaException("A hierarquia não pode ter mais de 5 níveis.");
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
}