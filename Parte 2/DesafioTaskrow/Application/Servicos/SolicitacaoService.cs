using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using DesafioTaskrow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class SolicitacaoService : ISolicitacaoService
{
    private readonly Contexto _context;

    public SolicitacaoService(Contexto contexto)
    {
        _context = contexto;
    }

    public async Task<List<SolicitacaoRetorno>> ObterSolicitacoes()
    {
        var solicitacoes = await _context.Solicitacoes.Select(x => new SolicitacaoRetorno
        {
            Id = x.Id,
            DataConclusao = x.DataConclusao,
            Descricao = x.Descricao,
            GrupoSolicitanteId = x.GrupoSolicitanteId,
            Status = x.Status,
            TipoSolicitacaoId = x.TipoSolicitacaoId,
            DataCriacao = x.DataCriacao
        }).ToListAsync();

        return solicitacoes;
    }

    public async Task CriarSolicitacao(SolicitacaoDto solicitacao)
    {
        bool existeGrupoSolicitante =
            await _context.GruposSolicitantes.AnyAsync(x => x.Id == solicitacao.GrupoSolicitanteId);

        if (!existeGrupoSolicitante)
        {
            throw new KeyNotFoundException("Grupo solicitante não encontrado.");
        }

        bool existeTipoSolicitacao =
            await _context.TiposSolicitacoes.AnyAsync(x => x.Id == solicitacao.TipoSolicitacaoId);

        if (!existeTipoSolicitacao)
        {
            throw new KeyNotFoundException("Tipo de solicitação não encontrado.");
        }

        int mes = DateTime.Now.Month;
        int ano = DateTime.Now.Year;

        var limiteMensal = _context.LimitesGrupos
            .FirstOrDefault(x => x.GrupoSolicitanteId == solicitacao.GrupoSolicitanteId && x.Ano == ano && x.Mes == mes)
            ?.LimiteMensal;
        
        if (limiteMensal is null)
        {
            Guid? grupoPaiId = _context.GruposSolicitantes.FirstOrDefault(x => x.Id == solicitacao.GrupoSolicitanteId)
                ?.GrupoSolicitantePaiId;

            if (grupoPaiId is not null)
            {
                limiteMensal = await _context.LimitesGrupos
                    .Where(x => x.GrupoSolicitante.GrupoSolicitantePaiId == grupoPaiId && x.Ano == ano && x.Mes == mes)
                    .Select(x => x.LimiteMensal)
                    .FirstOrDefaultAsync();
            }
            
        }
        
        // Nao foi cadastrado e nem tem limite no grupo pai
        if (limiteMensal is null)
        {
            throw new KeyNotFoundException("Você precisa cadastrar um limite mensal para essa solicitação");
        }
        
        int quantidadeMensalSolicitacaoGrupo = await _context.Solicitacoes
            .CountAsync(x => x.GrupoSolicitanteId == solicitacao.GrupoSolicitanteId 
                             && x.DataCriacao.Year == ano 
                             && x.DataCriacao.Month == mes);

        if (quantidadeMensalSolicitacaoGrupo >= limiteMensal)
        {
            throw new LimiteSolicitacaoExcedidoException("O limite de solicitações criadas para esse grupo no mês atual já foi excedido");
        }
        
        var novaSolicitacao = new Solicitacao
        {
            Status = solicitacao.Status,
            GrupoSolicitanteId = solicitacao.GrupoSolicitanteId,
            TipoSolicitacaoId = solicitacao.TipoSolicitacaoId,
            DataConclusao = solicitacao.DataConclusao,
            Descricao = solicitacao.Descricao,
        };

        _context.Solicitacoes.Add(novaSolicitacao);
        await _context.SaveChangesAsync();
    }

    public Task RemoverSolicitacao(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task EditarSolicitacao(Guid id, SolicitacaoDto solicitacao)
    {
        throw new NotImplementedException();
    }
}