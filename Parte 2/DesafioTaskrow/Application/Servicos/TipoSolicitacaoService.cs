using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using DesafioTaskrow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class TipoSolicitacaoService : ITipoSolicitacaoService
{
    private readonly Contexto _contexto;

    public TipoSolicitacaoService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<TipoSolicitacaoRetorno>> ObterTiposSolicitacao()
    {
        var tiposSolicitacao = await _contexto.TiposSolicitacoes.Select(x => new TipoSolicitacaoRetorno
        {
            Id = x.Id,
            Nome = x.Nome,
            Ativo = x.Ativo,
        }).ToListAsync();

        return tiposSolicitacao;
    }

    public async Task CriarTiposSolicitacao(string nome)
    {
        var tipoSolicitacao = new TipoSolicitacao()
        {
            Nome = nome,
        };

        await _contexto.TiposSolicitacoes.AddAsync(tipoSolicitacao);
        await _contexto.SaveChangesAsync();
    }

    public async Task EditarTiposSolicitacao(Guid id, string nome, bool ativo)
    {
        var tipoSolicitacao = await _contexto.TiposSolicitacoes.FindAsync(id);

        if (tipoSolicitacao == null)
        {
            throw new KeyNotFoundException("Tipo de Solicitação não encontrado.");
        }

        tipoSolicitacao.Nome = nome;
        tipoSolicitacao.Ativo = ativo;

        _contexto.TiposSolicitacoes.Update(tipoSolicitacao);
        await _contexto.SaveChangesAsync();
    }

    public async Task ExcluirTiposSolicitacao(Guid id)
    {
        var tipoSolicitacao = await _contexto.TiposSolicitacoes.FindAsync(id);
        bool existeSolicitacaoAssociada = await _contexto.Solicitacoes.AnyAsync(x => x.TipoSolicitacaoId == id);

        if (tipoSolicitacao == null)
        {
            throw new KeyNotFoundException("Tipo de Solicitação não encontrado.");
        }

        if (existeSolicitacaoAssociada)
        {
            throw new TipoSolicitacaoTemSolicitacaoException(
                "Esse tipo de solicitação tem uma solicitação associada. Não foi possível excluir, porém esse tipo de solicitação foi desativado");
        }

        _contexto.TiposSolicitacoes.Remove(tipoSolicitacao);
        await _contexto.SaveChangesAsync();
    }
}