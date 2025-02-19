using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using DesafioTaskrow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class LimiteService : ILimiteService
{
    private readonly Contexto _context;

    public LimiteService(Contexto contexto)
    {
        _context = contexto;
    }

    public async Task<List<LimiteGrupoRetorno>> ObterLimitesGrupos()
    {
        var limites = await _context.LimitesGrupos.Select(x => new LimiteGrupoRetorno
        {
            Id = x.Id,
            Ano = x.Ano,
            GrupoSolicitanteId = x.GrupoSolicitanteId,
            LimiteMensal = x.LimiteMensal,
            Mes = x.Mes,
            TipoSolicitacaoId = x.TipoSolicitacaoId
        }).ToListAsync();

        return limites;
    }

    public async Task<Guid> CriarLimiteGrupo(LimiteGrupoDto limiteGrupo)
    {
        bool limiteJaExiste = await _context.LimitesGrupos.AnyAsync(x =>
            x.Ano == limiteGrupo.Ano && x.Mes == limiteGrupo.Mes &&
            x.GrupoSolicitanteId == limiteGrupo.GrupoSolicitanteId &&
            x.TipoSolicitacaoId == limiteGrupo.TipoSolicitacaoId);

        if (limiteJaExiste)
        {
            throw new LimiteJaExisteException("Limite ja existe para esse grupo e esse tipo de solicitação");
        }

        var limite = new LimiteGrupo()
        {
            Ano = limiteGrupo.Ano,
            Mes = limiteGrupo.Mes,
            GrupoSolicitanteId = limiteGrupo.GrupoSolicitanteId,
            TipoSolicitacaoId = limiteGrupo.TipoSolicitacaoId,
            LimiteMensal = limiteGrupo.LimiteMensal,
        };
        _context.LimitesGrupos.Add(limite);
        await _context.SaveChangesAsync();

        return limite.Id;
    }

    public async Task EditarLimiteGrupo(Guid id, LimiteGrupoDto limiteGrupo)
    {
        int quantidadeSolicitacoesMes = await _context.Solicitacoes.CountAsync(x =>
            x.TipoSolicitacaoId == id && x.GrupoSolicitanteId == limiteGrupo.GrupoSolicitanteId &&
            x.DataCriacao.Month == limiteGrupo.Mes && x.DataCriacao.Year == limiteGrupo.Ano);

        if (limiteGrupo.LimiteMensal < quantidadeSolicitacoesMes)
        {
            throw new Exception("A quantidade de solicitações já cadastradas é maior que a informada na edição");
        }
        
        var limite = await _context.LimitesGrupos.FirstOrDefaultAsync(x => x.Id == id);

        if (limite == null)
        {
            throw new KeyNotFoundException("Limite não encontrado");
        }
        
        limite.LimiteMensal = limiteGrupo.LimiteMensal;
        limite.Mes = limiteGrupo.Mes;
        limite.Ano = limiteGrupo.Ano;
        limite.GrupoSolicitanteId = limiteGrupo.GrupoSolicitanteId;
        limite.TipoSolicitacaoId = limiteGrupo.TipoSolicitacaoId;
        
        _context.LimitesGrupos.Update(limite);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverLimiteGrupo(Guid id)
    {
        var limite = await _context.LimitesGrupos.FirstOrDefaultAsync(x => x.Id == id);
        
        if (limite == null)
        {
            throw new KeyNotFoundException("Limite não encontrado");
        }
        
        int quantidadeSolicitacoesMes = await _context.Solicitacoes.CountAsync(x =>
            x.TipoSolicitacaoId == id && x.GrupoSolicitanteId == limite.GrupoSolicitanteId &&
            x.DataCriacao.Month == limite.Mes && x.DataCriacao.Year == limite.Ano);

        if (quantidadeSolicitacoesMes > 0)
        {
            throw new Exception("Já existe solicitações dentro desse limite, não pode ser removido");
        }
        
        _context.LimitesGrupos.Remove(limite);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LimiteGrupoRetorno>>  ObterLimiteGrupoPorTipoSolicitacaoMesEspecifico(Guid idGrupo, Guid idTipo, int ano, int mes)
    {
        var limites = await _context.LimitesGrupos.Where(x =>
            x.Ano == ano && x.Mes == mes &&
            x.GrupoSolicitanteId == idGrupo &&
            x.TipoSolicitacaoId == idTipo)
            .Select(x => new LimiteGrupoRetorno
            {
                Id = x.Id,
                Ano = x.Ano,
                GrupoSolicitanteId = x.GrupoSolicitanteId,
                LimiteMensal = x.LimiteMensal,
                Mes = x.Mes,
                TipoSolicitacaoId = x.TipoSolicitacaoId
            })
            .ToListAsync();
        
        return limites;
    }
}