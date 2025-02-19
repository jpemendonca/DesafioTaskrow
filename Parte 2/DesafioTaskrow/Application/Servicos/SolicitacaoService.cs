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
    private readonly IVerificacaoSolicitacaoService _verificacaoService;

    public SolicitacaoService(Contexto contexto, IVerificacaoSolicitacaoService verificacaoService)
    {
        _context = contexto;
        _verificacaoService = verificacaoService;
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

    public async Task<Guid> CriarSolicitacao(SolicitacaoDto solicitacao)
    {
        await _verificacaoService.VerificarSolicitacao(solicitacao);
        
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
        
        return novaSolicitacao.Id;
    }

    public async Task RemoverSolicitacao(Guid id)
    {
        var solicitacao = _context.Solicitacoes.FirstOrDefault(x => x.Id == id);

        if (solicitacao is null)
        {
            throw new KeyNotFoundException("Solicitação não encontrada");
        }
        
        _context.Solicitacoes.Remove(solicitacao);
        await _context.SaveChangesAsync();
    }

    public async Task EditarSolicitacao(Guid id, SolicitacaoDto solicitacao)
    {
        var solicitacaoCadastrada  = await _context.Solicitacoes.FirstOrDefaultAsync(x => x.Id == id);
        
        if (solicitacaoCadastrada is null)
        {
            throw new KeyNotFoundException("Solicitação não encontrada");
        }
        
        await _verificacaoService.VerificarSolicitacao(solicitacao);
        
        solicitacaoCadastrada.Descricao = solicitacao.Descricao;
        solicitacaoCadastrada.GrupoSolicitanteId = solicitacao.GrupoSolicitanteId;
        solicitacaoCadastrada.Status = solicitacao.Status;
        solicitacaoCadastrada.TipoSolicitacaoId = solicitacao.TipoSolicitacaoId;
        solicitacaoCadastrada.DataConclusao = solicitacao.DataConclusao;
        
        _context.Solicitacoes.Update(solicitacaoCadastrada);
        await _context.SaveChangesAsync();
    }
}