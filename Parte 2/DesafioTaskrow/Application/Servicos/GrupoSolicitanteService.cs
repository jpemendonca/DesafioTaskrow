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

    public async Task<GrupoSolicitanteRetorno?> ObterPorId(Guid id)
    {
        var grupo = await _contexto.GruposSolicitantes.Select(x => new GrupoSolicitanteRetorno
        {
            Id = x.Id,
            Nome = x.Nome,
            GrupoPaiId = x.GrupoSolicitantePaiId
        }).FirstOrDefaultAsync(x => x.Id == id);

        return grupo;
    }

    public async Task<List<GrupoSolicitanteRetorno>> ObterGruposSolicitantesFilhos(Guid grupoPaiId)
    {
        var filhos = await _contexto.GruposSolicitantes
            .Where(x => x.GrupoSolicitantePaiId == grupoPaiId)
            .Select(x => new GrupoSolicitanteRetorno
            {
                Id = x.Id,
                Nome = x.Nome,
                GrupoPaiId = x.GrupoSolicitantePaiId,
            })
            .ToListAsync();

        return filhos;
    }

    public async Task<Guid> CriarGrupoSolicitante(GrupoSolicitanteDto grupoSolicitanteDto)
    {
        if (string.IsNullOrWhiteSpace(grupoSolicitanteDto.Nome))
        {
            throw new NomeObrigatorioException("O nome do grupo solicitante é obrigatório.");
        }
        
        if(grupoSolicitanteDto.GrupoPaiId is not null)
        {
            await _verificacaoService.VerificarGrupoPaiExiste(grupoSolicitanteDto.GrupoPaiId.Value); // Se nao houver grupo pai lança um erro
            await _verificacaoService.VerificarHierarquia(grupoSolicitanteDto.GrupoPaiId.Value); // Lança um erro se houver problemas
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

    public async Task EditarGrupoSolicitante(Guid id, GrupoSolicitanteDto grupoSolicitanteDto)
    {
        var grupo = await _contexto.GruposSolicitantes.FindAsync(id);
    
        if (grupo is null)
        {
            throw new GrupoNaoEncontradoException("O grupo solicitante não foi encontrado.");
        }
    
        if (string.IsNullOrWhiteSpace(grupoSolicitanteDto.Nome))
        {
            throw new NomeObrigatorioException("O nome do grupo solicitante é obrigatório.");
        }

        if (grupoSolicitanteDto.GrupoPaiId == id)
        {
            throw new HierarquiaCiclicaException("O grupo não pode ser seu próprio pai.");
        }

        if (grupoSolicitanteDto.GrupoPaiId is not null)
        {
            await _verificacaoService.VerificarGrupoPaiExiste(grupoSolicitanteDto.GrupoPaiId.Value);
            await _verificacaoService.VerificarHierarquia(grupoSolicitanteDto.GrupoPaiId.Value);
        }

        grupo.Nome = grupoSolicitanteDto.Nome;
        grupo.GrupoSolicitantePaiId = grupoSolicitanteDto.GrupoPaiId;
    
        await _contexto.SaveChangesAsync();
    }


    public async Task RemoverGrupoSolicitante(Guid id)
    {
        var grupo = await _contexto.GruposSolicitantes.FindAsync(id);

        if (grupo is null)
        {
            throw new GrupoNaoEncontradoException("O grupo solicitante não foi encontrado.");
        }
        
        _contexto.GruposSolicitantes.Remove(grupo);
        await _contexto.SaveChangesAsync();
    }
}